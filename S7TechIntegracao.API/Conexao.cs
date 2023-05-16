using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;

namespace S7TechIntegracao.API
{
    public class Conexao
    {
        private static readonly Conexao _instancia = new Conexao();

        public static Conexao GetInstance()
        {
            try
            {
                return _instancia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Conexao()
        {
            var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
            DefaultUri = param["HanaApi"];
        }

        public RestClient Client { get; set; }

        public string DefaultUri { get; set; }

        public DateTime? ExpirationDate { get; set; } = null;
        public string SessionId { get; set; }

        public void Login(bool altUser = false)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];
                var companyDB = param["CompanyDB"];
                var userName = altUser? param["AltUserName"] : param["UserName"];
                var password = altUser? param["AltPassword"] : param["Password"];

                if (ExpirationDate.HasValue && DateTime.Now < ExpirationDate.Value)
                    return;

                Client = new RestClient(hanaApi);
                Client.CookieContainer = new CookieContainer();
                Client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                Client.Timeout = -1;

                var request = new RestRequest("Login", Method.POST);
                var login = new Login(companyDB, userName, password);
                request.AddJsonBody(login);

                var response = Client.Execute<HanaSession>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var hanaSession = response.Data;
                var cookies = Client.CookieContainer;

                cookies.SetCookies(new Uri(hanaApi), $"B1SESSION={hanaSession.SessionId}");
                SessionId = hanaSession.SessionId;
                ExpirationDate = DateTime.Now.AddMinutes(hanaSession.SessionTimeout);

                Log4Net.Log.Info($"B1SESSION = {hanaSession.SessionId}  ExpirationDate = {ExpirationDate}");

            }
            catch (Exception ex)
            {
                Log4Net.Log.Error(ex.Message);
                throw ex;
            }
        }

        public void Logout()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                Client = new RestClient(hanaApi);
                Client.CookieContainer = new CookieContainer();
                Client.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                Client.Timeout = -1;

                var request = new RestRequest("Logout", Method.POST);
                var response = Client.Execute(request);

                if (response.StatusCode != HttpStatusCode.NoContent)
                    throw new Exception(response.Content);

                ExpirationDate = null;
            }
            catch (Exception e)
            {
                Log4Net.Log.Error($"[Conexao] [Logout] {e.Message}");

                throw e;
            }
        }
    }
}