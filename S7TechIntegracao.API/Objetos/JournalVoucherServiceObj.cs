using Dapper;
using Newtonsoft.Json;
using RestSharp;
using S7TechIntegracao.API.Controllers;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class JournalVoucherServiceObj
    {

        private static readonly JournalVoucherServiceObj _instancia = new JournalVoucherServiceObj();

        public static JournalVoucherServiceObj GetInstance()
        {
            return _instancia;
        }

     
        public JournalVouchersService_Add Cadastrar(object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                Log4Net.Log.Info($"[Diagnostics] [Cadastrar] {model}");

                var json = JsonConvert.SerializeObject(model);

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("JournalVouchersService_Add", Method.POST);
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                var response = client.Execute<JournalVouchersService_Add>(request);

                if (response.StatusCode != HttpStatusCode.NoContent)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[JournalVoucherServiceObj] [Cadastrar] {ex.Message}");
                throw ex;
            }
        }

        public List<JournalEntry> ConsultarTodasTransacoesContabeis()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("JournalEntries", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<JournalEntry>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[JournalVoucherServiceObj] [ConsultarTodasTransacoesContabeis] {ex.Message}");

                throw ex;
            }
        }



        public JournalEntry ConsultarTransacaoContabil(int transId)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"JournalEntries({transId})", Method.GET);
                var response = client.Execute<JournalEntry>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[JournalVoucherServiceObj] [ConsultarTransacaoContabil] {ex.Message}");

                throw ex;
            }
        }

        public List<object> ConsultarGatilhoArea(string costingCode2)
        {
            try
            {
                var ret = new List<object>();

                var query = string.Format(S7Tech.GetConsultas("ConsultarGatilhoArea"), costingCode2);

                using (var hanaService = new HanaService())
                    ret = hanaService.GetHanaConnection().Query<object>(query).ToList();

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[JournalVoucherServiceObj] [ConsultarGatilhoArea] {ex.Message}");

                throw ex;
            }
        }

        public List<object> ConsultarGatilhoSegmento(string projectCode)
        {
            try
            {
                var ret = new List<object>();

                var query = string.Format(S7Tech.GetConsultas("ConsultarGatilhoSegmento"), projectCode);

                using (var hanaService = new HanaService())
                    ret = hanaService.GetHanaConnection().Query<object>(query).ToList();

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[JournalVoucherServiceObj] [ConsultarGatilhoSegmento] {ex.Message}");

                throw ex;
            }
        }
    }
}