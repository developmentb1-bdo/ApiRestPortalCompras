using Dapper;
using Newtonsoft.Json;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;

namespace S7TechIntegracao.API.Objetos
{
    public class ApprovalRequestParameterObj
    {


        private static readonly ApprovalRequestParameterObj _instancia = new ApprovalRequestParameterObj();

        public static ApprovalRequestParameterObj GetInstance()
        {
            return _instancia;
        }

        public S7T_APROVOPRQ GetByCode(string code)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var request = new RestRequest($"S7T_APROVOPRQ('{code}')", Method.GET);

                var client = Conexao.GetInstance().Client;
                var response = client.Execute<string>(request);

                var b1Session = client.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                Log4Net.Log.Debug($"[ApprovalRequestParameterObj] [GetByCode] [Sessão {b1Session.Value}]");

                var ret = JsonConvert.DeserializeObject<S7T_APROVOPRQ>(response.Data);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] {ex.Message}");
                throw ex;
            }
        }

        public S7T_APROVOPRQ GetByCostCenterCode(string costCenter)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var request = new RestRequest($"S7T_APROVOPRQ?$filter=U_CentroCusto eq '{costCenter}'", Method.GET);

                var client = Conexao.GetInstance().Client;
                var response = client.Execute<string>(request);

                var b1Session = client.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                Log4Net.Log.Debug($"[ApprovalRequestParameterObj] [GetByCode] [Sessão {b1Session.Value}]");

                var ret = JsonConvert.DeserializeObject<OData_APROVOPRQ>(response.Data);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return ret.Value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] {ex.Message}");
                throw ex;
            }
        }

        public List<S7T_APROVOPRQ> GetByEmployeeId(string empId)
        {
            try
            {
                return ConsultarApprovalRequestParameter(empId);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] {ex.Message}");
                throw ex;
            }
        }

        //TODO: Criar controler para Parametros de Alcada
        public S7T_APROVOPRQ AlterarAlcadaAprovacaoInterna(S7T_APROVOPRQ s7APROVOPRQ)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                //Para verificar se existe pedidos ou solicitações                
                var codeGestor = ConsultarSolicitacoesComprasPendentesAprovacao(s7APROVOPRQ);
                var codeEmpdId = ConsultarPedidosComprasPendentesAprovacao(s7APROVOPRQ);

                if (codeGestor.Count != 0)
                {
                    throw new Exception("Existe Solicitação Pendente de aprovação!");
                }
                else if (codeEmpdId.Count != 0)
                {
                    throw new Exception("Existe Pedido de Compra Pendente!");
                }
                else
                {
                    var request = new RestRequest($"S7T_APROVOPRQ('{s7APROVOPRQ.Code}')", Method.PATCH);

                    var model = JsonConvert.SerializeObject(s7APROVOPRQ);

                    request.AddParameter("application/json", model, ParameterType.RequestBody);
                    var client = Conexao.GetInstance().Client;
                    var response = client.Execute<string>(request);

                    var b1Session = client.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                    Log4Net.Log.Debug($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] [Sessão {b1Session.Value}]");

                    if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                        throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                    return GetByCode(s7APROVOPRQ.Code);
                }

            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] {ex.Message}");
                throw ex;
            }
        }

        private List<Documents> ConsultarSolicitacoesComprasPendentesAprovacao(S7T_APROVOPRQ model)
        {
            try
            {
                var ret = new List<Documents>();

                var codApr = ConsultaCodApr(model);
                var centroCusto = model.U_CentroCusto;

                var query = string.Format(S7Tech.GetConsultas("ConsultarSolicitacoesComprasPendentesAprovacao"), codApr);

                using (var hanaService = new HanaService())
                {
                    try
                    {
                        ret = hanaService.GetHanaConnection().Query<Documents>(query).Where(x => x.CentroCusto == centroCusto).ToList();
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
                Log4Net.Log.Error($"[PurchaseRequestsObj] [ConsultarSolicitacoesComprasPendentesAprovacao] {ex.Message}");
                throw ex;
            }
        }

        private List<Documents> ConsultarPedidosComprasPendentesAprovacao(S7T_APROVOPRQ model)
        {
            try
            {
                var ret = new List<Documents>();

                var codApr = ConsultaCodApr(model);
                var centroCusto = model.U_CentroCusto;

                var query = string.Format(S7Tech.GetConsultas("ConsultarPedidosComprasPendentesAprovacao"), codApr);

                using (var hanaService = new HanaService())
                {
                    try
                    {
                        ret = hanaService.GetHanaConnection().Query<Documents>(query).Where(x => x.CentroCusto == centroCusto).ToList();
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

        private string ConsultaCodApr(S7T_APROVOPRQ model)
        {
            var code = model.S7T_APROVPRQ1Collection.FirstOrDefault().Code;
            var lineId = model.S7T_APROVPRQ1Collection.FirstOrDefault().LineId;

            return GetByCode(code).S7T_APROVPRQ1Collection.Where(x => x.LineId == lineId).FirstOrDefault().U_CodApr;
        }

        private List<S7T_APROVOPRQ> ConsultarApprovalRequestParameter(string empId)
        {
            try
            {
                var request = new RestRequest($"S7T_APROVOPRQ", Method.GET);

                var client = Conexao.GetInstance().Client;
                var response = client.Execute<string>(request);

                var ret = JsonConvert.DeserializeObject<OData_APROVOPRQ>(response.Data);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);


                var filteredRet = ret.Value.Where(x => x.S7T_APROVPRQ1Collection.Any(w => w.U_CodApr == empId)).ToList();

                return filteredRet;

            }
            catch (Exception ex)
            {
   
                throw ex;
            }
        }
    }
}