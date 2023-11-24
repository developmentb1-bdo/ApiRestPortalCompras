using Dapper;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Extensions;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class ApprovalRequestsObj
    {


        private static readonly ApprovalRequestsObj _instancia = new ApprovalRequestsObj();

        public static ApprovalRequestsObj GetInstance()
        {
            return _instancia;
        }

        public S7T_OWDD AdicionarRegraAprovacaoInterna(int draftEntry)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                //var docEsboco = DraftsObj.GetInstance().Consultar(draftEntry);
                var s7OWDD = GerarObjetoAprovacao(draftEntry);

                if (s7OWDD.Aprovadores.Count == 0)
                {
                    throw new Exception("Não Existe Alçada de aprovação!");
                }
                else
                {
                    var model = JsonConvert.SerializeObject(s7OWDD);

                    var client = Conexao.GetInstance().Client;

                    dynamic ret = "";

                    var b1Session = client.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                    Log4Net.Log.Debug($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] [Sessão {b1Session.Value}]");

                    var request = new RestRequest($"S7T_OWDD", Method.POST);
                    request.AddParameter("application/json", model, ParameterType.RequestBody);
                    var response = client.Execute<string>(request);                  

                    dynamic validError = JsonConvert.DeserializeObject(response.Content); 

                    var errorCode = validError["error"];
                    string codeError = Convert.ToString(validError["error"]);

                    if (!string.IsNullOrEmpty(codeError))
                    {
                        var retDadosSL = errorCode["message"];
                        var retDadosSLCode = errorCode["code"];

                        if (retDadosSLCode == 301 || retDadosSLCode == 401)
                        {
                            //logout usuário corrente da session
                            Conexao.GetInstance().Logout();
                            //login usuário alternativo
                            Conexao.GetInstance().Login();

                            var sessionId = Conexao.GetInstance().SessionId;

                            var client1 = Conexao.GetInstance().Client;

                            var b1Session1 = client1.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                            Log4Net.Log.Debug($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] [Sessão {b1Session1.Value}]");

                            request = new RestRequest($"S7T_OWDD", Method.POST);
                            request.AddParameter("application/json", model, ParameterType.RequestBody);
                            response = client1.Execute<string>(request);                          
                        }  
                        else
                        {
                            throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);
                        }
                    }
                    else
                    {
                        var query = string.Format(S7Tech.GetConsultas("ConsultarRegraAprovacaoVazia"), draftEntry, s7OWDD.U_ObjType);

                        using (var hanaService = new HanaService())
                        {
                            var dt = hanaService.ExecuteDataTable(query);

                            if (dt.Rows.Count != 0)
                            {
                                //logout usuário corrente da session
                                Conexao.GetInstance().Logout();
                                //login usuário alternativo
                                Conexao.GetInstance().Login();

                                var sessionId = Conexao.GetInstance().SessionId;

                                var client1 = Conexao.GetInstance().Client;

                                var b1Session1 = client1.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                                Log4Net.Log.Debug($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] [Sessão {b1Session1.Value}]");


                                request = new RestRequest($"S7T_OWDD", Method.POST);
                                request.AddParameter("application/json", model, ParameterType.RequestBody);
                                response = client1.Execute<string>(request);
                            }
                        }
                    }                   

                    if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                        throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                    ret = JsonConvert.DeserializeObject<S7T_OWDD>(response.Data);

                    return ret;
                }
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [AdicionarRegraAprovacaoInterna] {ex.Message} Regra de Aprovação Interna não Inserida DraftEntry:{draftEntry}");
                throw ex;
            }
        }

        public S7T_OWDD ConsultarRegraAprovacaoInterna(string code)
        {
            try
            {
                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"S7T_OWDD('{code}')", Method.GET);
                var response = client.Execute<string>(request);

                var ret = JsonConvert.DeserializeObject<S7T_OWDD>(response.Data);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [ConsultarRegraAprovacaoInterna] {ex.Message}");
                throw ex;
            }
        }

        public S7T_OWDD ConsultarRegraAprovacaoInternaByDraftEntry(int draftEntry)
        {
            try
            {
                var code = ConsultarCodeAlcadaAprovacao(draftEntry);
                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"S7T_OWDD('{code}')", Method.GET);
                var response = client.Execute<string>(request);

                var ret = JsonConvert.DeserializeObject<S7T_OWDD>(response.Data);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [ConsultarRegraAprovacaoInternaByDraftEntry] {ex.Message}");
                throw ex;
            }
        }

        private S7T_OWDD GerarObjetoAprovacao(int draftEntry)
        {
            try
            {
                var docEsboco = DraftsObj.GetInstance().Consultar(draftEntry);
                var s7OWDD = new S7T_OWDD()
                {
                    Code = GetNextCode(),
                    U_DraftEntry = docEsboco.DocEntry,
                    U_DocDate = docEsboco.DocDate.Value,
                    U_IsDraft = "Y",
                    U_DocEntry = -1,
                    U_MaxRejReqr = 1,
                    U_MaxReqr = 1,
                    U_Status = "W",
                    U_ObjType = (docEsboco.DocObjectCode == "oPurchaseOrders" ? "22" : "1470000113")
                };

                var tipoDocumento = s7OWDD.U_ObjType;
                var x = 0;

                if (tipoDocumento == "22")
                {
                    var tipoDoc = s7OWDD.U_ObjType == "22" ? "2" : "1";
                    var query = string.Empty;

                    var grupoCentroCusto = docEsboco.DocumentLines.GroupBy(t => t.CostingCode2).ToList();

                    foreach (var item in grupoCentroCusto)
                    {
                        var somaCentroCusto = docEsboco.DocumentLines.Where(i => i.CostingCode2 == item.Key).Sum(t => t.LineTotal.Value);
                        query = string.Format(S7Tech.GetConsultas("ConsultarLinhaRegraAprovacao"), somaCentroCusto.ToString(CultureInfo.InvariantCulture), item.Key, tipoDoc);

                        using (var hanaService = new HanaService())
                        {
                            var lineId = hanaService.GetHanaConnection().Query<int>(query).LastOrDefault();

                            query = string.Format(S7Tech.GetConsultas("ConsultarRegraAprovacao"), lineId, item.Key, tipoDoc);

                            using (var dt = hanaService.ExecuteDataTable(query))
                            {
                                foreach (System.Data.DataRow row in dt.Rows)
                                {
                                    var s7WDD1 = new S7T_WDD1()
                                    {
                                        Code = s7OWDD.Code,
                                        LineId = x,
                                        U_empID = Convert.ToInt32(row["empID"]),
                                        U_UserID = Convert.ToInt32(row["userId"]),
                                        U_CentroCusto = item.Key
                                    };

                                    s7OWDD.Aprovadores.Add(s7WDD1);
                                    x++;
                                }
                            }
                        }
                    }
                    var valorDoc = docEsboco.DocTotal;
                    var query1 = string.Empty;
                    using (var hanaService = new HanaService())
                    {
                        query1 = string.Format(S7Tech.GetConsultas("ConsultarRegraAprovacaoPorValor"), valorDoc.ToString(CultureInfo.InvariantCulture));
                        using (var dt = hanaService.ExecuteDataTable(query1))
                        {
                            foreach (System.Data.DataRow row in dt.Rows)
                            {
                                var s7WDD1 = new S7T_WDD1()
                                {
                                    Code = s7OWDD.Code,
                                    LineId = x,
                                    U_empID = Convert.ToInt32(row["empID"]),
                                    U_UserID = Convert.ToInt32(row["userId"])
                                };
                                s7OWDD.Aprovadores.Add(s7WDD1);
                                x++;
                            }
                        }
                    }
                }
                else
                {
                    var tipoDoc = s7OWDD.U_ObjType == "1470000113" ? "1" : "2";
                    var query2 = string.Empty;

                    var grupoCentroCusto = docEsboco.DocumentLines.GroupBy(t => t.CostingCode2).ToList();

                    foreach (var item in grupoCentroCusto)
                    {
                        using (var hanaService = new HanaService())
                        {
                            query2 = string.Format(S7Tech.GetConsultas("ConsultarRegraAprovacaoSolicitacaoCompras"), item.Key, tipoDoc);
                            using (var dt = hanaService.ExecuteDataTable(query2))
                            {
                                foreach (System.Data.DataRow row in dt.Rows)
                                {
                                    var s7WDD1 = new S7T_WDD1()
                                    {
                                        Code = s7OWDD.Code,
                                        LineId = x,
                                        U_empID = Convert.ToInt32(row["empID"]),
                                        U_UserID = Convert.ToInt32(row["userId"]),
                                        U_CentroCusto = item.Key
                                    };

                                    s7OWDD.Aprovadores.Add(s7WDD1);
                                    x++;
                                }
                            }
                        }
                    }

                }

                s7OWDD.U_MaxReqr = s7OWDD.Aprovadores.Count;

                return s7OWDD;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [GerarObjetoAprovacao] {ex.Message} DraftEntry:{draftEntry}");
                throw ex;
            }
        }

        private string ConsultarCodeAlcadaAprovacao(int draftEntry)
        {
            try
            {
                var ret = string.Empty;
                var query = string.Format(S7Tech.GetConsultas("ConsultarCodeRegraAprovacaoInterna"), draftEntry);

                using (var hanaService = new HanaService())
                    ret = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [ConsultarCodeAlcadaAprovacao] {ex.Message}");
                throw ex;
            }
        }

        internal List<EmployeeInfo> ConsultarAprovadoresAlcadaCustomizada(int draftEntry)
        {
            try
            {
                var docEsboco = DraftsObj.GetInstance().Consultar(draftEntry);
                var listaEmployeesInfo = new List<EmployeeInfo>();

                var query = string.Format(S7Tech.GetConsultas("ConsultarRegraAprovacao"), docEsboco.DocTotal.ToString(CultureInfo.InvariantCulture), docEsboco.DocumentLines.FirstOrDefault().CostingCode2);

                using (var hanaService = new HanaService())
                {
                    using (var dt = hanaService.ExecuteDataTable(query))
                    {
                        foreach (System.Data.DataRow row in dt.Rows)
                        {
                            var employee = new EmployeeInfo()
                            {
                                EmployeeID = Convert.ToInt32(row["empID"]),
                                eMail = Convert.ToString(row["email"]),
                                ApplicationUserID = Convert.ToInt32(row["userId"])
                            };

                            listaEmployeesInfo.Add(employee);
                        }
                    }
                }

                return listaEmployeesInfo;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [ConsultarAprovadoresAlcadaCustomizada] {ex.Message}");
                throw ex;
            }
        }

        private string GetNextCode()
        {
            try
            {
                var query = S7Tech.GetConsultas("ConsultarProximoCodeS7T_OWDD");
                var code = "";

                using (var hanaService = new HanaService())
                    code = Convert.ToString(hanaService.ExecuteScalar(query));

                return code;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [GetNextCode] {ex.Message}");
                throw ex;
            }
        }

        private int GetwddCode(int draftKey)
        {
            try
            {
                var query = string.Format(S7Tech.GetConsultas("ConsultarCodigoAprovacao"), draftKey);

                using (var hanaService = new HanaService())
                {
                    var wddCode = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                    if (!string.IsNullOrEmpty(wddCode))
                        return int.Parse(wddCode);
                }

                return 0;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [Aprovar] {ex.Message}");
                throw ex;
            }
        }

        public void Aprovar(int draftKey)
        {

            Conexao.GetInstance().Login();

            var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
            var hanaApi = param["HanaApi"];

            try
            {
                int wddCode = GetwddCode(draftKey);

                var paramsModel = ParamsModel.GetInstance();

               

                var aprovacao = new ApprovalRequest
                {
                    Status = "arsApproved"
                };

                aprovacao.ApprovalRequestDecisions.Add(new ApprovalRequestDecision
                {
                    Status = "ardApproved",
                    Remarks = "Aprovado via Portal de compras",
                    ApproverUserName = paramsModel.UserName,
                    ApproverPassword = paramsModel.Password
                });

                using (var hanaService = new HanaService())
                {
                    var query = string.Format(S7Tech.GetConsultas("ValidaGeracaoDocumento"), draftKey);                  

                    using (var dt = hanaService.ExecuteDataTable(query))
                    {
                        foreach (System.Data.DataRow row in dt.Rows)
                        {

                            var s7OWDD = new S7T_OWDD() {
                                U_DraftEntry = Convert.ToInt32(row["DraftEntry"]),
                                U_Status = Convert.ToString(row["Status"])
                            };
                            var numberDoc = s7OWDD.U_DraftEntry;
                            var status = s7OWDD.U_Status;

                            if (numberDoc == 0)
                            {
                                var model = JsonConvert.SerializeObject(aprovacao);
                                var client = Conexao.GetInstance().Client;
                                var request = new RestRequest($"ApprovalRequests({wddCode})", Method.PATCH);
                                request.AddParameter("application/json", model, ParameterType.RequestBody);
                                var response = client.Execute(request);

                                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                                {
                                    Conexao.GetInstance().Logout();
                                    //login usuário alternativo
                                    Conexao.GetInstance().Login(true);

                                    DraftsObj.GetInstance().AdicionarEsbocoAprovado(draftKey);

                                    Conexao.GetInstance().Logout();
                                    //login usuário alternativo
                                    Conexao.GetInstance().Login();
                                }
                                else
                                {
                                    dynamic validError = JsonConvert.DeserializeObject(response.Content);
                                    var errorCode = validError["error"];
                                    string codeError = Convert.ToString(validError["error"]);

                                    if (!string.IsNullOrEmpty(codeError))
                                    {
                                        var retDadosSL = errorCode["message"];
                                        var retDadosSLCode = errorCode["code"];

                                        if (retDadosSLCode == 301 || retDadosSLCode == 401)
                                        {
                                           
                                            Conexao.GetInstance().Logout();
                                            
                                            Conexao.GetInstance().Login();

                                            var sessionId = Conexao.GetInstance().SessionId;

                                            var client1 = Conexao.GetInstance().Client;

                                            var b1Session1 = client1.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                                            Log4Net.Log.Debug($"[ApprovalRequestsObj] [Aprovar] [Sessão {b1Session1.Value}]");

                                            model = JsonConvert.SerializeObject(aprovacao);

                                            request = new RestRequest($"ApprovalRequests({wddCode})", Method.PATCH);
                                            request.AddParameter("application/json", model, ParameterType.RequestBody);
                                            response = client1.Execute(request);

                                            if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                                                throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                                            Conexao.GetInstance().Logout();
                                            //login usuário alternativo
                                            Conexao.GetInstance().Login(true);

                                            DraftsObj.GetInstance().AdicionarEsbocoAprovado(draftKey);

                                            Conexao.GetInstance().Logout();
                                            //login usuário alternativo
                                            Conexao.GetInstance().Login();

                                        }
                                        else
                                        {
                                            throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);
                                        }
                                    }
                                }                           
                                
                            }
                            else if ((numberDoc != 0 && status == "W"))
                            {
                                var model = JsonConvert.SerializeObject(aprovacao);
                                var client = Conexao.GetInstance().Client;
                                var request = new RestRequest($"ApprovalRequests({wddCode})", Method.PATCH);
                                request.AddParameter("application/json", model, ParameterType.RequestBody);
                                var response = client.Execute<string>(request);
                                                             
                                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                                {
                                    Conexao.GetInstance().Logout();
                                    //login usuário alternativo
                                    Conexao.GetInstance().Login(true);

                                    DraftsObj.GetInstance().AdicionarEsbocoAprovado(draftKey);

                                    Conexao.GetInstance().Logout();
                                    //login usuário alternativo
                                    Conexao.GetInstance().Login();

                                    status = "Y";
                                }                              
                                else
                                {
                                    dynamic validError = JsonConvert.DeserializeObject(response.Content);
                                    var errorCode = validError["error"];
                                    string codeError = Convert.ToString(validError["error"]);

                                    if (!string.IsNullOrEmpty(codeError))
                                    {
                                        var retDadosSL = errorCode["message"];
                                        var retDadosSLCode = errorCode["code"];

                                        if (retDadosSLCode == 301 || retDadosSLCode == 401)
                                        {
                                            //logout usuário corrente da session
                                            Conexao.GetInstance().Logout();
                                            //login usuário alternativo
                                            Conexao.GetInstance().Login();

                                            var sessionId = Conexao.GetInstance().SessionId;

                                            var client1 = Conexao.GetInstance().Client;

                                            var b1Session1 = client1.CookieContainer.GetCookies(new Uri(hanaApi))["B1SESSION"];
                                            Log4Net.Log.Debug($"[ApprovalRequestsObj] [Aprovar] [Sessão {b1Session1.Value}]");

                                            model = JsonConvert.SerializeObject(aprovacao);                                          
                                            request = new RestRequest($"ApprovalRequests({wddCode})", Method.PATCH);
                                            request.AddParameter("application/json", model, ParameterType.RequestBody);
                                            response = client1.Execute<string>(request);

                                            if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                                                throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                                            Conexao.GetInstance().Logout();
                                            //login usuário alternativo
                                            Conexao.GetInstance().Login(true);

                                            DraftsObj.GetInstance().AdicionarEsbocoAprovado(draftKey);

                                            Conexao.GetInstance().Logout();
                                            //login usuário alternativo
                                            Conexao.GetInstance().Login();
                                        }
                                        else
                                        {
                                            throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Conexao.GetInstance().Logout();
                                //login usuário alternativo
                                Conexao.GetInstance().Login(true);

                                DraftsObj.GetInstance().AdicionarEsbocoAprovado(draftKey);

                                Conexao.GetInstance().Logout();
                                //login usuário alternativo
                                Conexao.GetInstance().Login();
                            }
                        }
                    }

                   
                }              

                var esboco = DraftsObj.GetInstance().Consultar(draftKey);
                var solicitante = EmployeesInfoObj.GetInstance().Consultar(esboco.DocumentsOwner.Value);
                RetornarEmail(solicitante.eMail, true, draftKey, esboco.DocObjectCode);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [Aprovar] {ex.Message}");
                throw ex;
            }
        }

        public void Reprovar(int draftKey, string mensagem)
        {
            Conexao.GetInstance().Login();
            try
            {
                int wddCode = GetwddCode(draftKey);

                var aprovacao = new ApprovalRequest
                {
                    Status = "arsNotApproved"
                };

                aprovacao.ApprovalRequestDecisions.Add(new ApprovalRequestDecision()
                {
                    Status = "ardNotApproved",
                    Remarks = mensagem
                });

                var model = JsonConvert.SerializeObject(aprovacao);

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"ApprovalRequests({wddCode})", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var query = string.Format(S7Tech.GetConsultas("ValidaReprovacaoDocumento"), draftKey);

                using (var hanaService = new HanaService())
                {
                    var wddCode1 = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                    if (wddCode1 != null)
                    {
                        //logout usuário corrente da session
                        Conexao.GetInstance().Logout();
                        //login usuário alternativo
                        Conexao.GetInstance().Login();

                        var client1 = Conexao.GetInstance().Client;
                        var request1 = new RestRequest($"ApprovalRequests({wddCode1})", Method.PATCH);
                        request1.AddParameter("application/json", model, ParameterType.RequestBody);
                        var response1 = client1.Execute(request1);

                        if (!response1.IsSuccessful && response1.StatusCode != System.Net.HttpStatusCode.NoContent)
                            throw new Exception(!string.IsNullOrEmpty(response1.ErrorMessage) ? response1.ErrorMessage : response1.Content);
                    }
                }

                var esboco = DraftsObj.GetInstance().Consultar(draftKey);
                var solicitante = EmployeesInfoObj.GetInstance().Consultar(esboco.DocumentsOwner.Value);
                RetornarEmail(solicitante.eMail, false, draftKey, esboco.DocObjectCode);

            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [Reprovar] {ex.Message}");
                throw ex;
            }
        }

        public string ConsultarEmailGestorArea(string centroCusto)
        {
            try
            {
                var query = string.Format(S7Tech.GetConsultas("ConsultarEmailGestorArea"), centroCusto);

                using (var hanaService = new HanaService())
                {
                    var email = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                    return email;
                }
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [ConsultarEmailGestorArea] {ex.Message}");
                throw ex;
            }
        }

        internal List<AprovadorEsboco> ConsultarStatusAprovadoresEsboco(int draftEntry)
        {
            try
            {
                var query = string.Format(S7Tech.GetConsultas("ConsultarAprovadoresEsboco"), draftEntry);

                using (var hanaService = new HanaService())
                {
                    var ret = hanaService.GetHanaConnection().Query<AprovadorEsboco>(query).ToList();

                    return ret;
                }
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [ConsultarStatusAprovadoresEsboco] {ex.Message}");
                throw ex;
            }
        }

        private void RetornarEmail(string emailSolicitante, bool aprovado, int draftKey, string docObjectCode)
        {
            try
            {
                var tipoEmail = aprovado ? "Aprovado" : "Reprovado";

                var assunto = docObjectCode == "oPurchaseOrders" ? $"Pedido {draftKey} foi {tipoEmail}" : $"Solicitação {draftKey} foi {tipoEmail}";
                var corpoEmail = docObjectCode == "oPurchaseOrders" ? $"Seu pedido de compra nº {draftKey} foi {tipoEmail}!" : $"Sua solicitação nº {draftKey} foi {tipoEmail}!";


                var emails = new Emails()
                {
                    CorpoEmail = corpoEmail,
                    Assunto = assunto
                };

                var listaEmails = new List<string>() { emailSolicitante };

                EmailsObj.GetInstance().EnviarEmail(emails, listaEmails);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ApprovalRequestsObj] [RetornarEmail] {ex.Message}");
                throw ex;
            }
        }
    }
}