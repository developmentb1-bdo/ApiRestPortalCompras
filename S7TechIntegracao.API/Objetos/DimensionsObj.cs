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
    public class DimensionsObj
    {
        

        private static readonly DimensionsObj _instancia = new DimensionsObj();

        public static DimensionsObj GetInstance()
        {
            return _instancia;
        }

        public List<Dimension> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("Dimensions?$filter=IsActive eq 'tYES'", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Dimension>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPlacesObj] [ConsultarTodos] {ex.Message}");
                throw ex;
            }
        }

        public Dimension Consultar(int id)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Dimensions({id})", Method.GET);
                var response = client.Execute<Dimension>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPlacesObj] [Consultar] {ex.Message}");
                throw ex;
            }
        }
    }
}