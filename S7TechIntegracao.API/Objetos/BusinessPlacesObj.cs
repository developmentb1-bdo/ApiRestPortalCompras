using Dapper;
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
    public class BusinessPlacesObj
    {
        

        private static readonly BusinessPlacesObj _instancia = new BusinessPlacesObj();

        public static BusinessPlacesObj GetInstance()
        {
            return _instancia;
        }

        public List<BusinessPlace> ConsultarTodos()
        {
            try
            {
                var ret = new List<BusinessPlace>();
                var query = S7Tech.GetConsultas("ConsultarFiliais");

                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<BusinessPlace>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPlacesObj] [ConsultarTodos] {ex.Message}");
                throw ex;
            }
        }

        //public List<BusinessPlace> ConsultarTodos()
        //{
        //    try
        //    {
        //        var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
        //        var hanaApi = param["HanaApi"];

        //        var client = Conexao.GetInstance().Client;
        //        var request = new RestRequest("BusinessPlaces?$filter=Disabled eq 'tNO' ", Method.GET);
        //        var response = client.Execute<RetornoListaGenerica<List<BusinessPlace>>>(request);

        //        if (!response.IsSuccessful)
        //            throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

        //        return response.Data.value.OrderBy(x => x.BPLID).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4Net.Log.Error($"[BusinessPlacesObj] [ConsultarTodos] {ex.Message}");
        //        throw ex;
        //    }
        //}

        public BusinessPlace Consultar(int id)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"BusinessPlaces({id})?$filter=Disabled eq 'tNO'", Method.GET);
                var response = client.Execute<BusinessPlace>(request);

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