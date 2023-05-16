using Dapper;
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
    public class PurchaseInvoicesObj
    {


        private static readonly PurchaseInvoicesObj _instancia = new PurchaseInvoicesObj();

        public static PurchaseInvoicesObj GetInstance()
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

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("PurchaseInvoices", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                //InserirInfItemDocumento(response.Data);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseInvoicesObj] [Cadastrar] {ex.Message}");

                throw ex;
            }
        }


        private void InserirInfItemDocumento(Documents documents)
        {
            try
            {

                foreach (var item in documents.DocumentLines)
                {
                    var code = GetNextCode();
                    var tipoDocumento = "OPCH";
                    var query = string.Format(S7Tech.GetConsultas("InserirInfItemDocumento"), code, item.U_S7T_NAT_BC_CRED, item.U_RegPC, item.U_IND_OPER, item.U_IND_ORIG_CRED, tipoDocumento, item.DocEntry, item.ItemCode, item.LineNum);
                    using (var hanaService = new HanaService())
                        hanaService.GetHanaConnection().ExecuteScalar(query);
                }

            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseInvoicesObj] [InserirInfItemDocumento] {ex.Message}");

                throw ex;
            }
        }
        private string GetNextCode()
        {
            try
            {
                var query = S7Tech.GetConsultas("ConsultarProximoCodeSKILL_INF_ITEM_DOC");
                var code = "";

                using (var hanaService = new HanaService())
                    code = Convert.ToString(hanaService.ExecuteScalar(query));

                return code;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseInvoicesObj] [GetNextCode] {ex.Message}");
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
                var request = new RestRequest("PurchaseInvoices", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseInvoicesObj] [ConsultarTodos] {ex.Message}");

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
                var request = new RestRequest($"PurchaseInvoices({docEntry})", Method.GET);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseInvoicesObj] [Consultar] {ex.Message}");

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
                var request = new RestRequest($"PurchaseInvoices?$filter=DocNum eq {docNum}", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseInvoicesObj] [ConsultarPorDocNum] {ex.Message}");

                throw ex;
            }
        }

        public List<Documents> ConsultarPorNumAtCard(string numAtCard)
        {
            try
            {
                var ret = new List<Documents>();
                var retDocEntry = new List<int>();
                var query = string.Format(S7Tech.GetConsultas("ConsultarNumAtCardPurchaseInvoice"), numAtCard);
                using (var hanaService = new HanaService())
                {
                    retDocEntry = hanaService.GetHanaConnection().Query<int>(query).ToList();
                }

                foreach (var docEntry in retDocEntry)
                {
                    ret.Add(Consultar(docEntry));
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseInvoicesObj] [ConsultarPorNumAtCard] {ex.Message}");

                throw ex;
            }
        }
    }
}