using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class PurchaseDeliveryNotesObj
    {
        

        private static readonly PurchaseDeliveryNotesObj _instancia = new PurchaseDeliveryNotesObj();

        public static PurchaseDeliveryNotesObj GetInstance()
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
                var request = new RestRequest("PurchaseDeliveryNotes", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseDeliveryNotesObj] [Cadastrar] {ex.Message}");

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
                var request = new RestRequest("PurchaseDeliveryNotes", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseDeliveryNotesObj] [ConsultarTodos] {ex.Message}");

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
                var request = new RestRequest($"PurchaseDeliveryNotes({docEntry})", Method.GET);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseDeliveryNotesObj] [Consultar] {ex.Message}");

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
                var request = new RestRequest($"PurchaseDeliveryNotes?$filter=DocNum eq {docNum}", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[PurchaseDeliveryNotesObj] [ConsultarPorDocNum] {ex.Message}");

                throw ex;
            }
        }
    }
}