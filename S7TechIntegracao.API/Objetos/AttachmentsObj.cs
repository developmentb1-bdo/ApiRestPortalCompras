using Dapper;
using Newtonsoft.Json;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace S7TechIntegracao.API.Objetos
{
    public class AttachmentsObj
    {
        

        private static readonly AttachmentsObj _instancia = new AttachmentsObj();

        public static AttachmentsObj GetInstance()
        {
            return _instancia;
        }

        public Attachment Cadastrar(object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("Attachments2", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<Attachment>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[AttachmentsObj] [Cadastrar] {ex.Message}");
                throw ex;
            }
        }

        public List<Attachment> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("Attachments2", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Attachment>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[AttachmentsObj] [ConsultarTodos] {ex.Message}");
                throw ex;
            }
        }

        public Attachment Consultar(int docEntry)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Attachments2({docEntry})", Method.GET);
                var response = client.Execute<Attachment>(request);


                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[AttachmentsObj] [Consultar] {ex.Message}");
                throw ex;
            }
        }

        public string ConsultarPastaAnexo()
        {
            try
            {
                var query = string.Format(S7Tech.GetConsultas("ConsultarPastaAnexo"));

                using (var hanaService = new HanaService())
                {
                    var path = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                    return path;
                }
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[AttachmentsObj] [ConsultarPastaAnexo] {ex.Message}");
                throw ex;
            }
        }


        public void UpdateAttachments(int absEntry, object document)
        {
            try
            {
                var client = Conexao.GetInstance().Client;

                

                var model = JsonConvert.SerializeObject(document);
                

                var request = new RestRequest($"Attachments2({absEntry})", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<string>(request);

                if (response.StatusCode != System.Net.HttpStatusCode.NoContent)
                    throw new Exception(response.Content);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[AttachmentsObj] [UpdateAttachments] {ex.Message}");

                throw ex;
            }
        }
    }
}