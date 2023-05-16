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
    public class ProjectsObj
    {
        

        private static readonly ProjectsObj _instancia = new ProjectsObj();

        public static ProjectsObj GetInstance()
        {
            return _instancia;
        }

        public List<Project> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("Projects?$filter=Active eq 'tYES'", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Project>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ProjectsObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }

        public Project Consultar(string code)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Projects('{code}')", Method.GET);
                var response = client.Execute<Project>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ProjectsObj] [Consultar] {ex.Message}");

                throw ex;
            }
        }

        public List<Project> ConsultarPorCode(string code, int top)
        {
            try
            {
                var ret = new List<Project>();

                var strTop = string.Empty;

                if (top != -1)
                    strTop = $"TOP {top}";

                var query = string.Format(S7Tech.GetConsultas("ConsultarProjetosPortalCompras"), code, strTop);
                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<Project>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[ProjectsObj] [ConsultarPorCode] {ex.Message}");

                throw ex;
            }
        }
    }
}