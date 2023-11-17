using RestSharp;
using S7TechIntegracao.API.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Utils;

namespace S7TechIntegracao.API.Objetos
{
    public class DraftsObj
    {
        

        private static readonly DraftsObj _instancia = new DraftsObj();

        public static DraftsObj GetInstance()
        {
            return _instancia;
        }

        public object ConsultarUltimoDocEntry(int solicitante)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Drafts?$top=1&$orderby=DocEntry desc&$filter=Requester eq '{solicitante}'", Method.GET);
                var response = client.Execute<object>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DraftsObj] [ConsultarUltimoDocEntry] {ex.Message}");

                throw ex;
            }
        }

        public object ConsultarPorGuid(string guid)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Drafts?$top=1&$orderby=DocNum desc&$filter=U_S7T_IdPortal eq '{guid}'", Method.GET);
                var response = client.Execute<object>(request);
                                

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DraftsObj] [ConsultarPorGuid] {ex.Message}");

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
                var request = new RestRequest($"Drafts({docEntry})", Method.GET);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DraftsObj] [Consultar] {ex.Message}");

                throw ex;
            }
        }

        public void AdicionarEsbocoAprovado(int docEntry)
        {

            //logout usuário corrente da session
            Conexao.GetInstance().Logout();
            //login usuário alternativo
            Conexao.GetInstance().Login(true);

            try
            {
                var esbocoOriginal = Consultar(docEntry);

                var document = new JObject();
                var docEntryJObject = new JObject();               
                        

                docEntryJObject["DocEntry"] = docEntry;
               string docDate = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

                docEntryJObject.Add("TaxDate", docDate);

                document["Document"] = docEntryJObject;
  

                var modelo = JsonConvert.SerializeObject(document);

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"DraftsService_SaveDraftToDocument", Method.POST);
                request.AddParameter("application/json", modelo, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (!response.IsSuccessful && response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var query = string.Format(S7Tech.GetConsultas("ValidaGeracaoDocumento"), docEntry);

                using (var hanaService = new HanaService())
                {
                    var wddCode = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                    if(wddCode != null)
                    {
                        //logout usuário corrente da session
                        Conexao.GetInstance().Logout();
                        //login usuário alternativo
                        Conexao.GetInstance().Login();

                        var client1 = Conexao.GetInstance().Client;
                        var request1 = new RestRequest($"DraftsService_SaveDraftToDocument", Method.POST);
                        request1.AddParameter("application/json", modelo, ParameterType.RequestBody);
                        var response1 = client1.Execute(request1);

                        if (!response1.IsSuccessful && response1.StatusCode != System.Net.HttpStatusCode.NoContent)
                            throw new Exception(!string.IsNullOrEmpty(response1.ErrorMessage) ? response1.ErrorMessage : response1.Content);
                    }
                }

            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DraftsObj] [AdicionarEsbocoAprovado] {ex.Message}");

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
                var request = new RestRequest($"Drafts?$filter=DocNum eq {docNum}", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DraftsObj] [ConsultarPorDocNum] {ex.Message}");

                throw ex;
            }
        }

        public void Atualizar(int draftEntry, object document)
        {
            try
            {
                var client = Conexao.GetInstance().Client;

                var model = JsonConvert.SerializeObject(document);

                var request = new RestRequest($"Drafts({draftEntry})", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<string>(request);

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DraftsObj] [Atualizar] {ex.Message}");

                throw ex;
            }
        }

        public Documents PegarUltimoDocumentoEsboco()
        {
            try
            {
                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Drafts?$select=DocEntry&$orderby=DocEntry desc&$top=1", Method.GET);
                var response = client.Execute<string>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var ret = JsonConvert.DeserializeObject<OData_Generic>(response.Data);

                var docEntry = ret.Value.FirstOrDefault().DocEntry;

                return Consultar(docEntry);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DraftsObj] [ConsultarPorDocNum] {ex.Message}");

                throw ex;
            }
        }
    }
}