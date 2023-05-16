using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class ParamsModel
    {
        private static readonly ParamsModel _instancia = new ParamsModel();

        public static ParamsModel GetInstance()
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

        public string HanaApi { get; set; }
        public string Server { get; set; }
        public string DbUserName { get; set; }
        public string DbPassword { get; set; }
        public string LicenseServer { get; set; }
        public string CompanyDB { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int DbServerType { get; set; }
        public string StringConnection { get; set; }

        public string Url { get; set; }

        public ParamsModel()
        {

        }
    }
}