using Dapper;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class ItemsObj
    {
        

        private static readonly ItemsObj _instancia = new ItemsObj();

        public static ItemsObj GetInstance()
        {
            return _instancia;
        }

        public Items Cadastrar(object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("Items", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<Items>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [Cadastrar] {ex.Message}");

                throw ex;
            }
        }
       
        public List<Items> ConsultarTodos()
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
                var request = new RestRequest("Items", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Items>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }

        public Items Consultar(string itemCode)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Items({itemCode})", Method.GET);
                var response = client.Execute<Items>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [Consultar] {ex.Message}");

                throw ex;
            }
        }

        public List<object> ConsultarPorItemCode(string itemCode, int top)
        {
            try
            {
                var ret = new List<object>();

                var strTop = string.Empty;

                if (top != -1)
                    strTop = $"TOP {top}";

                var query = string.Format(S7Tech.GetConsultas("ConsultarItensPortalCompras"), itemCode, "", strTop);
                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<object>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [ConsultarPorItemCode] {ex.Message}");

                throw ex;
            }
        }

        public List<object> ConsultarPorItemName(string itemName, int top)
        {
            try
            {
                var ret = new List<object>();

                var strTop = string.Empty;

                if (top != -1)
                    strTop = $"TOP {top}";

                var query = string.Format(S7Tech.GetConsultas("ConsultarItensPortalCompras"), "", itemName, strTop);
                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<object>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [ConsultarPorItemName] {ex.Message}");

                throw ex;
            }
        }

        public List<Items> ConsultarPorTipo(string itemEstoque, string itemCompra, string itemVenda)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                itemEstoque = itemEstoque == "Y" ? "tYES" : "tNO";
                itemCompra = itemCompra == "Y" ? "tYES" : "tNO";
                itemVenda = itemVenda == "Y" ? "tYES" : "tNO";
                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Items?$filter=InventoryItem eq '{itemEstoque}' OR PurchaseItem eq '{itemCompra}' OR SalesItem eq '{itemVenda}'", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Items>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [ConsultarPorTipo] {ex.Message}");

                throw ex;
            }
        }

        public List<object> ConsultarTodos(string filtro)
        {
            try
            {
                var ret = new List<object>();

                var query = string.Format(S7Tech.GetConsultas("ConsultarItensPortalCompras"), "", "", "");
                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<object>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }

        public Items Atualizar(string itemCode, object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                client.Timeout = -1;
                var request = new RestRequest($"Items('{itemCode}')", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var item = Consultar(itemCode);

                return item;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ItemsObj] [Atualizar] {ex.Message}");

                throw ex;
            }
        }
    }
}