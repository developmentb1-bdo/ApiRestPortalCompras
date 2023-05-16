using Dapper;
using Newtonsoft.Json;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class SBOBobServiceObj
    {
        

        private static readonly SBOBobServiceObj _instancia = new SBOBobServiceObj();

        public static SBOBobServiceObj GetInstance()
        {
            return _instancia;
        }

        public List<Currency> GetCurrencies()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Currencies", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Currency>>>(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SBOBobServiceObj] [GetCurrencies] {ex.Message}");

                throw ex;
            }
        }
        public string GetSystemCurrency()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"SBOBobService_GetSystemCurrency", Method.POST);
                var response = client.Execute<string>(request);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Content;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SBOBobServiceObj] [ConsultarUltimoDocEntry] {ex.Message}");

                throw ex;
            }
        }
        public double GetCurrencyRate(string currency, DateTime date)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var model = JsonConvert.SerializeObject(new CurrencyInfo
                {
                    Currency = currency,
                    Date = date
                });

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"SBOBobService_GetCurrencyRate", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);

                var response = client.Execute<string>(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return Double.Parse(response.Content, CultureInfo.InvariantCulture);
                else
                    return TreatResponseData(response.Data);

                    //throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SBOBobServiceObj] [ConsultarUltimoDocEntry] {ex.Message}");

                throw ex;
            }
        }


        public List<PaymentMethod> GetPaymentMethods()
        {
            try
            {
                var ret = new List<PaymentMethod>();
                var query = S7Tech.GetConsultas("ConsultarCondicoesPagamento");

                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<PaymentMethod>(query).ToList();
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SBOBobServiceObj] [ConsultarUltimoDocEntry] {ex.Message}");

                throw ex;
            }
        }



        //public List<PaymentMethod> GetPaymentMethods()
        //{
        //    try
        //    {
        //        var client = Conexao.GetInstance().Client;
        //        var request = new RestRequest($"PaymentTermsTypes?$filter=U_S7T_ItemPortal eq 'Y'", Method.GET);
        //        var response = client.Execute<RetornoListaGenerica<List<PaymentMethod>>>(request);

        //        if (response.StatusCode != System.Net.HttpStatusCode.OK)
        //            throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

        //        return response.Data.value;
        //    }
        //    catch (Exception ex)
        //    {
        //        Log4Net.Log.Error($"[SBOBobServiceObj] [ConsultarUltimoDocEntry] {ex.Message}");

        //        throw ex;
        //    }
        //}
        private double TreatResponseData(string data)
        {
            try
            {
                var errorRoot = JsonConvert.DeserializeObject<ErrorRoot>(data);
                return errorRoot.error.code == -4006 ? 0 : 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}