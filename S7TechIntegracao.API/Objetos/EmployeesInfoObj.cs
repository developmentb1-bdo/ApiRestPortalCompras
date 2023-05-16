using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;

namespace S7TechIntegracao.API.Objetos 
{
    public class EmployeesInfoObj
    {
        

        private static readonly EmployeesInfoObj _instancia = new EmployeesInfoObj();

        public static EmployeesInfoObj GetInstance() 
        {
            return _instancia;
        }

        public EmployeeInfo Cadastrar(object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = new RestClient();
                var request = new RestRequest("EmployeesInfo", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<EmployeeInfo>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmployeesInfoObj] [Cadastrar] {ex.Message}");

                throw ex;
            }
        }

        public EmployeeInfo Atualizar(int employeeID, object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                client.Timeout = -1;
                var request = new RestRequest($"EmployeesInfo({employeeID})", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var businessPartiner = Consultar(employeeID);

                return businessPartiner;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmployeesInfoObj] [Atualizar] {ex.Message}");

                throw ex;
            }
        }

        public List<EmployeeInfo> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("EmployeesInfo", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<EmployeeInfo>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmployeesInfoObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }

        public EmployeeInfo Consultar(int employeeID)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"EmployeesInfo({employeeID})", Method.GET);
                var response = client.Execute<EmployeeInfo>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmployeesInfoObj] [Consultar] {ex.Message}");

                throw ex;
            }
        }

        public EmployeeInfo ConsultarEmail(string usuario, string senha)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;                
                var request = new RestRequest($"EmployeesInfo?$filter=U_S7T_CodUsuario eq '{usuario}'and Active eq 'tYES'", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<EmployeeInfo>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);                

                return response.Data.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmployeesInfoObj] [ConsultarEmail] {ex.Message}");

                throw ex;
            }
        }
    }
}