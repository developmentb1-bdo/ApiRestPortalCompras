using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class PurchaseOrdersObj
    {
        private static readonly PurchaseOrdersObj _instancia = new PurchaseOrdersObj();

        public static PurchaseOrdersObj GetInstance()
        {
            return _instancia;
        }

        public Documents Cadastrar(object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                DocumentsObj.GetInstance().AdicionarCentrosCustos(ref model);

                Log4Net.Log.Info($"[Diagnostics] [Cadastrar] {model}");

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("PurchaseOrders", Method.POST);
                request.AddHeader("Prefer", "return-no-content");
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [Cadastrar] {ex.Message}");

                throw ex;
            }
        }

        public List<Documents> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var ret = new
                {
                    value = new List<BusinessPartners>()
                };

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("PurchaseOrders", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }

        public Documents Consultar(int docEntry)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"PurchaseOrders({docEntry})", Method.GET);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [Consultar] {ex.Message}");

                throw ex;
            }
        }

        public Documents ConsultarPorDocNum(int docNum)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"PurchaseOrders?$filter=DocNum eq {docNum}", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarPorDocNum] {ex.Message}");

                throw ex;
            }
        }

        public List<Documents> ConsultarPorRequisitante(int solicitante, int pagina)
        {
            try
            {
                var ret = new List<Documents>();
                var query = string.Format(S7Tech.GetConsultas("ConsultarSolicitanteAdministrador"), solicitante);

                var limit = DefaultSettingsModel.GetInstance().PaginationLimit;

                var offSet = (pagina - 1) * limit;
                
                using (var hanaService = new HanaService())
                {
                    //var tipoUsuario = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                    query = string.Format(S7Tech.GetConsultas("ConsultarPedidoComprasPorRequisitantePortal"), solicitante, limit, offSet);

                    //if (tipoUsuario == "01")
                    //    query = query.Replace("--", "");

                    ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarPorRequisitanteColaborador] {ex.Message}");

                throw ex;
            }
        }

        public List<Documents> ConsultarPorRequisitanteStatus(int solicitante, int docEntry)
        {
            try
            {
                var ret = new List<Documents>();
                var query = string.Format(S7Tech.GetConsultas("ConsultarSolicitanteAdministrador"), solicitante);               

                using (var hanaService = new HanaService())
                {
                    query = string.Format(S7Tech.GetConsultas("ConsultarPedComprasPorRequisitantePortalStatus"), solicitante, docEntry);
                   
                    ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarPorRequisitanteStatus] {ex.Message}");

                throw ex;
            }
        }

        public List<Documents> ConsultarPorAprovador(int solicitante, DateTime dataDe, DateTime dataAte)
        {
            try
            {
                var ret = new List<Documents>();
            
                using (var hanaService = new HanaService())
                {
                    var query = string.Format(S7Tech.GetConsultas("ConsultarPedidoComprasPorAprovadorPortal"), solicitante, dataDe.ToString("yyyy-MM-dd"), dataAte.ToString("yyyy-MM-dd"));

                    ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarPorRequisitanteColaborador] {ex.Message}");

                throw ex;
            }
        }

        public List<Documents> ConsultarPedidosPorColaboradorPortal(int empId)
        {
            try
            {
                var ret = new List<Documents>();
                var query = string.Format(S7Tech.GetConsultas("ConsultarPedidosPorColaboradorPortal"), empId);

                using (var hanaService = new HanaService())
                    ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarPedidosPorColaboradorPortal] {ex.Message}");
                throw ex;
            }
        }

        public List<Documents> ConsultarPedidosComprasPendentesAprovacao(int empId)
        {
            try
            {
                var ret = new List<Documents>();
                var query = string.Format(S7Tech.GetConsultas("ConsultarPedidosComprasPendentesAprovacao"), empId);

                using (var hanaService = new HanaService())
                {
                    try
                    {
                        ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                    

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarPedidosComprasPendentesAprovacao] {ex.Message}");

                throw ex;
            }
        }

        public List<Documents> ConsultarPedidosComprasPendentesAprovacaoPorValor(int empId)
        {
            try
            {
                var ret = new List<Documents>();
                var query = string.Format(S7Tech.GetConsultas("ConsultarPedComprasPendentesAprovacaoPorValor"), empId);

                using (var hanaService = new HanaService())
                {
                    try
                    {
                        ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarPedidosComprasPendentesAprovacaoPorValor] {ex.Message}");

                throw ex;
            }
        }

        public int ConsultarDraftKey(int docEntry)
        {
            try
            {
                var ret = -1;
                var query = string.Format(S7Tech.GetConsultas("ConsultarDraftKeyOPOR"), docEntry);

                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<int>(query).FirstOrDefault();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [ConsultarDraftKey] {ex.Message}");

                throw ex;
            }
        }

        public void AprovarReprovarPedidoPendenteAprovacao(AprovacaoPedidoCompra aprovacao, string status)
        {
            Conexao.GetInstance().Login();   

            try
            {
                var dataAprovacao = DateTime.Now.ToString("yyyy-MM-dd");
                var horaAprovacao = DateTime.Now.ToString("HHmm");

                var query = string.Format(S7Tech.GetConsultas("AtualizarPedidoCompraPendenteAprovacaoLinhas"), status, aprovacao.EmpId, aprovacao.DraftEntry, aprovacao.Mensagem, aprovacao.CentroCusto, dataAprovacao, horaAprovacao);

                using (var hanaService = new HanaService())
                    hanaService.GetHanaConnection().Query(query);

                if (status == "N")
                    query = string.Format(S7Tech.GetConsultas("AtualizarPedidoCompraPendenteAprovacaoReprovado"), status, aprovacao.EmpId, aprovacao.DraftEntry, aprovacao.Mensagem, aprovacao.CentroCusto);

                using (var hanaService = new HanaService())
                    hanaService.GetHanaConnection().Query(query);                

                var s7OWDD = VerificarPedidoPodeSerAprovadoReprovado(aprovacao.DraftEntry);

                if (s7OWDD == null)
                    return;

                if (s7OWDD.U_Status == "Y")
                {
                    AtualizaDataDoDocumento(aprovacao.DraftEntry);
                    ApprovalRequestsObj.GetInstance().Aprovar(aprovacao.DraftEntry);
                }
                else if (s7OWDD.U_Status == "N")
                    ApprovalRequestsObj.GetInstance().Reprovar(aprovacao.DraftEntry, aprovacao.Mensagem);
                else
                    EnviarEmail(s7OWDD.Code);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [AprovarReprovarPedidoPendenteAprovacao] DraftEntry: {aprovacao.DraftEntry} \r\n {ex.Message}");

                throw ex;
            }
        }

        public void AprovarReprovarPedidoComprasPorValor(AprovacaoPedidoCompraPorValor aprovacao, string status)
        {
            Conexao.GetInstance().Login();

            try
            {
                var dataAprovacao = DateTime.Now.ToString("yyyy-MM-dd");
                var horaAprovacao = DateTime.Now.ToString("HHmm");

                var query = string.Format(S7Tech.GetConsultas("AtualizarPedidoCompraPendenteAprovacaoPorValor"), status, aprovacao.EmpId, aprovacao.DraftEntry, aprovacao.Mensagem, dataAprovacao, horaAprovacao);

                using (var hanaService = new HanaService())
                    hanaService.GetHanaConnection().Query(query);

                if (status == "N")
                    query = string.Format(S7Tech.GetConsultas("AtuPCPendenteAprovacaoReprovadoPorValor"), status, aprovacao.EmpId, aprovacao.DraftEntry, aprovacao.Mensagem);

                using (var hanaService = new HanaService())
                    hanaService.GetHanaConnection().Query(query);

                var s7OWDD = VerificarPedidoPodeSerAprovadoReprovado(aprovacao.DraftEntry);

                if (s7OWDD == null)
                    return;

                if (s7OWDD.U_Status == "Y")
                {
                    AtualizaDataDoDocumento(aprovacao.DraftEntry);
                    ApprovalRequestsObj.GetInstance().Aprovar(aprovacao.DraftEntry);
                }
                else if (s7OWDD.U_Status == "N")
                    ApprovalRequestsObj.GetInstance().Reprovar(aprovacao.DraftEntry, aprovacao.Mensagem);
                else
                    EnviarEmail(s7OWDD.Code);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [AprovarReprovarPedidoPorAlcadaValorPendenteAprovacao] DraftEntry: {aprovacao.DraftEntry} \r\n {ex.Message}");

                throw ex;
            }
        }

        public S7T_OWDD VerificarPedidoPodeSerAprovadoReprovado(int draftEntry)
        {
            try
            {
                var ret = new S7T_OWDD();
                var query = string.Format(S7Tech.GetConsultas("ConsultarPedidoCompraPodeSerAprovado"), draftEntry);

                using (var hanaService = new HanaService())
                {
                    hanaService.GetHanaConnection().ExecuteScalar(query);
                }

                using (var hanaService = new HanaService())
                {
                    query = string.Format(S7Tech.GetConsultas("ConsultarStatusAlcadaAprovacaoPedidoCompra"), draftEntry);
                    ret = hanaService.GetHanaConnection().Query<S7T_OWDD>(query).FirstOrDefault();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [VerificarPedidoPodeSerAprovadoReprovado] {ex.Message}");

                throw ex;
            }
        }
                
        private void AtualizarEmailEnviadoAlcadaAprovacao(string code, int empId, int lineId )
        {
            try
            {
                var query = string.Format(S7Tech.GetConsultas("AtualizarEmailEnviadoAprovador"), empId, code, lineId, "Y");
                using (var hanaService = new HanaService())
                    hanaService.GetHanaConnection().ExecuteScalar(query);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [AtualizarEmailEnviadoAlcadaAprovacao] {ex.Message}");

                throw ex;
            }
        }
        public void EnviarEmail(string codeS7Wdd)
        {
            try
            {
                var aprovacaoInterna = ApprovalRequestsObj.GetInstance().ConsultarRegraAprovacaoInterna(codeS7Wdd);
                var aprovadores = aprovacaoInterna.Aprovadores;

                var agrupamentoCentroCusto = aprovadores.GroupBy(x => x.U_CentroCusto).ToList();

                var attachmentsPath = AttachmentsObj.GetInstance().ConsultarPastaAnexo();

                foreach (var centroCusto in agrupamentoCentroCusto)
                {
                    if (aprovadores.Any(x => x.U_EmailEnv == "Y" && x.U_Status == "W" && x.U_CentroCusto == centroCusto.Key))
                        continue;

                    var proximoAprovador = aprovadores.Where(x => x.U_Status == "W" && x.U_CentroCusto == centroCusto.Key && (x.U_EmailEnv == "N" || x.U_EmailEnv == "")).OrderBy(x => x.LineId).FirstOrDefault();

                    if (proximoAprovador == null)
                        continue;

                    var emplooye = EmployeesInfoObj.GetInstance().Consultar(proximoAprovador.U_empID);
                    var docEsboco = DraftsObj.GetInstance().Consultar(aprovacaoInterna.U_DraftEntry);
                    var assunto = centroCusto.Key != null ? $"Aprovação de pedido de compras - {centroCusto.Key}" : $"Aprovação por alçada valor total do pedido de compras";
                    var corpoEmail = CriarCorpoEmail(docEsboco, emplooye, centroCusto.Key);

                    //Novo Layout Email
                    //var corpoEmail = EmailsObj.GetInstance().GetLayoutEmail(docEsboco.DocEntry, BoObjectTypes.oPurchaseOrders);

                    List<string> linhasAnexo = null;
                    if (docEsboco.AttachmentEntry.HasValue)
                    {
                        var anexos = AttachmentsObj.GetInstance().Consultar(docEsboco.AttachmentEntry.Value);
                        linhasAnexo = (
                            anexos.Attachments2_Lines
                                .Select(lines => $"{attachmentsPath}\\{lines.FileName}.{lines.FileExtension}")
                            ).ToList();
                    }

                    var emailAprovadores = new EmailsAprovadores()
                    {
                        Aprovador = proximoAprovador,
                        Assunto = assunto,
                        CorpoEmail = corpoEmail,
                        Anexos = linhasAnexo
                    };

                    EmailsObj.GetInstance().EnviarEmailAprovador(emailAprovadores);
                    AtualizarEmailEnviadoAlcadaAprovacao(aprovacaoInterna.Code, proximoAprovador.U_empID, proximoAprovador.LineId);
                }
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [EnviarEmail] {ex.Message}");

                throw ex;
            }
        }
        private string CriarCorpoEmail(Documents docEsboco, EmployeeInfo aprovador, string centroCusto)
        {
            try
            {
                var culturaBrasil = new CultureInfo("pt-BR");

                var documentsOwner = aprovador.EmployeeID;

                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var Url = param["url"];

                if (docEsboco.DocumentsOwner.HasValue)
                    documentsOwner = docEsboco.DocumentsOwner.Value;

                var employee = EmployeesInfoObj.GetInstance().Consultar(documentsOwner);

                var pagamento = SBOBobServiceObj.GetInstance().GetPaymentMethods().Where(x => x.GroupNumber == docEsboco.PaymentGroupCode).FirstOrDefault();

                var linhas = new StringBuilder();

                var linhasEsboco = docEsboco.DocumentLines.Where(x => x.CostingCode2 == centroCusto).ToList();

                if (linhasEsboco.Count > 0)
                {
                    foreach (var item in linhasEsboco)
                    {
                        linhas.AppendLine("<tr>");
                        linhas.AppendFormat("<td>{0}</td>", item.ItemCode);
                        linhas.AppendFormat("<td>{0}</td>", item.ItemDescription);
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.ShipDate.Value.ToString("dd/MM/yyyy"));
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.Quantity.Value.ToString(culturaBrasil));
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.UnitPrice.Value.ToString("N", culturaBrasil));
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.DiscountPercent.Value.ToString("N", culturaBrasil));
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.Price.Value.ToString("N", culturaBrasil));
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.LineTotal.Value.ToString("N", culturaBrasil));
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.RowTotalFC.Value.ToString("N", culturaBrasil));
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.ProjectCode);
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.CostingCode2);
                        linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.FreeText);
                        linhas.AppendLine("</tr>");
                    }
                }
                else
                {
                    var grupoCentroCusto = docEsboco.DocumentLines.GroupBy(t => t.CostingCode2).ToList();

                    foreach (var item1 in grupoCentroCusto)
                    {

                        linhasEsboco = docEsboco.DocumentLines.Where(x => x.CostingCode2 == item1.Key).ToList();

                        foreach (var item in linhasEsboco)
                        {
                            linhas.AppendLine("<tr>");
                            linhas.AppendFormat("<td>{0}</td>", item.ItemCode);
                            linhas.AppendFormat("<td>{0}</td>", item.ItemDescription);
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.ShipDate.Value.ToString("dd/MM/yyyy"));
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.Quantity.Value.ToString(culturaBrasil));
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.UnitPrice.Value.ToString("N", culturaBrasil));
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.DiscountPercent.Value.ToString("N", culturaBrasil));
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.Price.Value.ToString("N", culturaBrasil));
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.LineTotal.Value.ToString("N", culturaBrasil));
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.RowTotalFC.Value.ToString("N", culturaBrasil));
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.ProjectCode);
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.CostingCode2);
                            linhas.AppendFormat("<td style='text-align:center;'>{0}</td>", item.FreeText);
                            linhas.AppendLine("</tr>");
                        }
                    }
                }
                var dataSocilitacao = DateTime.Now;

                if (docEsboco.U_S7T_DataSolicitacao.HasValue)
                    dataSocilitacao = docEsboco.U_S7T_DataSolicitacao.Value;

                var linhaLink = "";
                if (centroCusto != null)
                {
                    linhaLink = $@"<a href=""{Url}AprovarPedido?draftEntry={docEsboco.DocEntry}&empID={aprovador.EmployeeID}&centroCusto={centroCusto}"" target=""_self"">Aprovar</a>  |  <a href=""{Url}ReprovarPedido?draftEntry={docEsboco.DocEntry}&empID={aprovador.EmployeeID}&centroCusto={centroCusto}"" target=""_self"" > Reprovar</a>";
                }
                else
                {
                    linhaLink = $@"<a href=""{Url}AprovarPedidoCompraPorValor?draftEntry={docEsboco.DocEntry}&empID={aprovador.EmployeeID}"" target=""_self"">Aprovar</a>  |  <a href=""{Url}ReprovarPedidoCompraPorValor?draftEntry={docEsboco.DocEntry}&empID={aprovador.EmployeeID}"" target=""_self"" > Reprovar</a>";
                } 

                var corpoEmail = $@"
                                    <html>
                                         <head>
                                           </head>
                                            <body>
                                                <form id=""form1"" method=""post"" action=""#"">
                                                    <div class=""container-fluid"" style=""border-style:groove; padding: 1%; margin: 1%; background - color:#F8F8FF;"">
                                                        <div class=""form-group"">
                                                            <div style = ""background -image: linear-gradient(to right, #4682B4, #B0C4DE, #B0C4DE);"">
                                                                <div class=""form-row col-12"" style=""padding: 1%"">
                                                                    <div class=""col-10"" >
                                                                        <strong>Pedido de compras</strong>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div style=""padding:1%"">
                                                            <div class=""form-row"">
                                                                <div class=""form-group col-lg-12"">
                                                                    <label>Documento: </label>
                                                                    <input type='text' value='{docEsboco.DocEntry}' />
                                                                </div>
                                                            </div>
                                                            <div class=""form-row"">
                                                                <div class=""form-group col-lg-2"">
                                                                    <label>Solicitante: </label>
                                                                    <input type='text' value='{employee.FirstName} {employee.LastName}' />
                                                                </div>
                                                                <div class=""form-group col-lg-3"">
                                                                    <label>Email: </label>
                                                                    <input type='text' value={employee.eMail} />
                                                                </div>
                                                                <div class=""form-group col-lg-3"">
                                                                    <label>Data solicitação: </label>
                                                                    <input type='text' value='{dataSocilitacao.ToString("dd/MM/yyyy")}' />
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class=""form-row"">
                                                            <div class=""form-group col-lg-6"" >
                                                                <label>Parceiro: </label>
                                                                <input type='text' value='{docEsboco.CardCode} - {docEsboco.CardName}' />
                                                            </div>
                                                            <div class=""form-group col-lg-2"" >
                                                                <label>Data de lançamento: </label>
                                                                <input type='text' value='{docEsboco.DocDate.Value.ToString("dd/MM/yyyy")}' />
                                                            </div>
                                                            <div class=""form-group col-lg-2"">
                                                                <label>Válido até: </label>
                                                                <input type='text' value='{docEsboco.DocDueDate.Value.ToString("dd/MM/yyyy")}' />
                                                            </div>
                                                            <div class=""form-group col-lg-4"" >
                                                                <label>Filial: </label>
                                                                <input type='text' value='{docEsboco.BPLName}' />
                                                            </div>
                                                        </div>
                                                        <div class=""form-row"">
                                                            <div class=""form -group col-lg-12"">
                                                                <label>Condição de Pagamento: </label>
                                                                <input type='text' value='{pagamento.PaymentTermsGroupName}' />
                                                            </div>
                                                        </div>
                                                        <div class=""form-row"">
                                                            <div class=""form -group col-lg-12"">
                                                                <label>Moeda: </label>
                                                                <input type='text' value='{docEsboco.DocCurrency}' />
                                                            </div>
                                                        </div>
                                                        <div class=""form-row"">
                                                            <div class=""form -group col-lg-12"">
                                                                <label>Total moeda estrangeira: </label>
                                                                <input type='text' value='{docEsboco.DocTotalFc.Value.ToString("N", culturaBrasil)}' />
                                                            </div>
                                                        </div>
                                                        <div class=""form-row"">
                                                            <div class=""form -group col-lg-12"">
                                                                <label>Total R$: </label>
                                                                <input type='text' value='{docEsboco.DocTotal.ToString("N", culturaBrasil)}' />
                                                            </div>
                                                        </div>                                                        
                                                        <div class=""form-row"">
                                                            <div class=""col-lg-12 table-responsive-xl"" >
                                                                <table id=""tbItemSolicCompra"" style=""white-space:nowrap; width:90%"" >
                                                                    <thead>
                                                                        <tr>
                                                                            <th>Item</th>
                                                                            <th>Descrição do item</th>
                                                                            <th>Data de entrega</th>
                                                                            <th>Quantidade</th>
                                                                            <th>Valor unitário</th>
                                                                            <th>Desconto%</th>
                                                                            <th>Valor após desc.</th>
                                                                            <th>Valor total R$</th>
                                                                            <th>Valor total moeda estrangeira</th>
                                                                            <th>Projeto</th>
                                                                            <th>Centro de custo</th>
                                                                            <th>Detalhes do item</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        {linhas.ToString()}
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </div>

                                                        <br />

                                                        <div class=""form-row"" >
                                                            <div class=""form-group col-5"" >
                                                                <label>Observação:</label>
                                                                <textarea rows=3>{docEsboco.Comments}</textarea>
                                                            </div>
                                                        </div>

                                                        <hr />

                                                        <div style=""margin-top:1%"" >
                                                            <div class=""form-group"">
                                                                {linhaLink}
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </form>
                                        </body>
                                    </html>
                         ";

                return corpoEmail;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [CriarCorpoEmail] {ex.Message}");

                throw ex;
            }
        }
        //Atualiza TaxDate quando parâmetro estiver marcado como True
        private void AtualizaDataDoDocumento(int draftEntry)
        {
            if (!DefaultSettingsModel.GetInstance().UpdateTaxDateNow)
                return;

            var document = new JObject();
            document["TaxDate"] = DateTime.Now.ToString("yyyy-MM-dd");

            DraftsObj.GetInstance().Atualizar(draftEntry, document);
        }
        public void UpdatePurchaseOrder(int docEntry, object document)
        {
            try
            {
                var client = Conexao.GetInstance().Client;

                var model = JsonConvert.SerializeObject(document);

                var request = new RestRequest($"PurchaseOrders({docEntry})", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<string>(request);

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(response.Content);

                var aprov = PurchaseOrdersObj.GetInstance().Consultar(docEntry);

                var solicitante = Convert.ToInt32(aprov.DocumentsOwner);
                var documento = Convert.ToInt32(aprov.DocEntry);

                var ret = new List<Documents>();
                using (var hanaService = new HanaService())
                {
                    var query = string.Format(S7Tech.GetConsultas("ConsultarDraftKeyPedido"), solicitante,documento);

                    ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                }

                var aprov2 = ret.FirstOrDefault().DocEntry;

                ApprovalRequestsObj.GetInstance().Aprovar(Convert.ToInt32(aprov2));

                var doc = Convert.ToInt32(aprov.DocNum);

                ret = new List<Documents>();
                using (var hanaService = new HanaService())
                {

                    var query = string.Format(S7Tech.GetConsultas("ConsultarCodeOWDD"), doc);

                    ret = hanaService.GetHanaConnection().Query<Documents>(query).ToList();                                 

                }

                var code = ret.FirstOrDefault().Code;

                var  query1 = string.Format(S7Tech.GetConsultas("AltualizarDraftEntryS7T_OWDD"), aprov2, code);
                using (var hanaService = new HanaService())
                    hanaService.GetHanaConnection().ExecuteScalar(query1);


            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseOrdersObj] [UpdatePurchaseOrder] {ex.Message}");

                throw ex;
            }
        }
    }
}