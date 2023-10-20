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
    public class ProfitCentersObj
    {
        

        private static readonly ProfitCentersObj _instancia = new ProfitCentersObj();

        public static ProfitCentersObj GetInstance()
        {
            return _instancia;
        }

        public List<ProfitCenter> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("ProfitCenters?$filter=Active eq 'tYES'", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<ProfitCenter>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ProfitCentersObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }

        public ProfitCenter Consultar(string code)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"ProfitCenters('{code}')?$filter=Active eq 'tYES'", Method.GET);
                var response = client.Execute<ProfitCenter>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ProfitCentersObj] [Consultar] {ex.Message}");

                throw ex;
            }
        }

        public List<DistributionRule> ConsultarPorCode(string code, int top)
        {
            try
            {
                var ret = new List<DistributionRule>();

                var strTop = string.Empty;

                if (top != -1)
                    strTop = $"TOP {top}";

                var query = string.Format(S7Tech.GetConsultas("ConsultarCentrosCustoPortalCompras"), code, strTop);
                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<DistributionRule>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ProfitCentersObj] [ConsultarPorCode] {ex.Message}");

                throw ex;
            }
        }
    }
}