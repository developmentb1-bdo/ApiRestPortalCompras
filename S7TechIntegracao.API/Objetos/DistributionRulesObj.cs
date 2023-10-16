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
    public class DistributionRulesObj
    {
        

        private static readonly DistributionRulesObj _instancia = new DistributionRulesObj();

        public static DistributionRulesObj GetInstance()
        {
            return _instancia;
        }

        public List<DistributionRule> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("DistributionRules?$filter=Active eq 'tYES'", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<DistributionRule>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DistributionRulesObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }

        public DistributionRule Consultar(string factorCode)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"DistributionRules('{factorCode}')?$filter=Active eq 'tYES'", Method.GET);
                var response = client.Execute<DistributionRule>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DistributionRulesObj] [Consultar] {ex.Message}");
                throw ex;
            }
        }

        public List<DistributionRule> ConsultarPorDimensao(DimensionOption option)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var dimensao = 0;

                if (option == DimensionOption.Dimensao1)
                    dimensao = 1;
                else if (option == DimensionOption.Dimensao2)
                    dimensao = 2;
                else if (option == DimensionOption.Dimensao3)
                    dimensao = 3;
                else if (option == DimensionOption.Dimensao4)
                    dimensao = 4;
                else if (option == DimensionOption.Dimensao5)
                    dimensao = 5;

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"DistributionRules?$filter=InWhichDimension eq {dimensao}", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<DistributionRule>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DistributionRulesObj] [ConsultarPorDimensao] {ex.Message}");
                throw ex;
            }
        }

        public List<object> ConsultarPorCode(string code, int top)
        {
            try
            {
                var ret = new List<object>();

                var strTop = string.Empty;

                if (top != -1)
                    strTop = $"TOP {top}";

                var query = string.Format(S7Tech.GetConsultas("ConsultarRegraDistribuicaoPortalCompras"), code, strTop);
                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<object>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DistributionRulesObj] [ConsultarPorDimensao] {ex.Message}");
                throw ex;
            }
        }
    }
}