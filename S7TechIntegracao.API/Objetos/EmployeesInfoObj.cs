using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Dapper;
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
        public EmployeeInfo GetByPassEncryptedUser(string usuario)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"EmployeesInfo?$select=U_S7T_SenhaPortal,U_S7T_PrimeiroAcesso&$filter=U_S7T_CodUsuario eq '{usuario}'and Active eq 'tYES'", Method.GET);
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

        public void AtualizarSenhaParaEncriptadaDoUsuario()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var employeeInfo = new List<EmployeeInfo>();

                using (var hanaService = new HanaService())
                {
                    var query = string.Format(S7Tech.GetConsultas("ConsultarColaborador"));
                    using (var dt = hanaService.ExecuteDataTable(query))
                    {
                        var user = hanaService.GetHanaConnection().Query<EmployeeInfo>(query).ToList();
                        foreach (var item in user)
                        {                           
                            string senha = item.U_S7T_SenhaPortal;
                            int employeeID = item.EmployeeID;
                            string codUsuarioPortal = item.U_S7T_CodUsuario;

                            string senhaCriptografada = EncryptStringAES(senha);

                            query = string.Format(S7Tech.GetConsultas("AtualizarUsuarioSenhaEncripitada"),senhaCriptografada, employeeID,codUsuarioPortal);
                            using (var update = new HanaService())
                                update.GetHanaConnection().ExecuteScalar(query);

                        }
                       
                    }
                    
                }               
                
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[EmployeesInfoObj] [Atualizar] {ex.Message}");

                throw ex;
            }
        }
        //private static byte[] _salt = Encoding.ASCII.GetBytes("o6806642kbM7c5");

        public static string EncryptStringAES(string plainText)
        {


            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(plainText);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
            //if (string.IsNullOrEmpty(plainText))
            //    throw new ArgumentNullException(nameof(plainText));
            //if (string.IsNullOrEmpty(sharedSecret))
            //    throw new ArgumentNullException(nameof(sharedSecret));
            //string str = (string)null;
            //RijndaelManaged rijndaelManaged = (RijndaelManaged)null;
            //try
            //{
            //    Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(sharedSecret, _salt);
            //    rijndaelManaged = new RijndaelManaged();
            //    rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
            //    ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
            //    using (MemoryStream memoryStream = new MemoryStream())
            //    {
            //        memoryStream.Write(BitConverter.GetBytes(rijndaelManaged.IV.Length), 0, 4);
            //        memoryStream.Write(rijndaelManaged.IV, 0, rijndaelManaged.IV.Length);
            //        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
            //        {
            //            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
            //                streamWriter.Write(plainText);
            //        }
            //        str = Convert.ToBase64String(memoryStream.ToArray());
            //    }
            //}
            //finally
            //{
            //    rijndaelManaged?.Clear();
            //}
            //return str;
        }
    }
}