using System.Collections.Specialized;
using System.Configuration;

namespace S7TechIntegracao.API.Utils {
    public static class ParametrosSAP {
        public static string CompanyDB { get; private set; }
        public static string ApiRemota { get; private set; }

        static ParametrosSAP() {
            NameValueCollection param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
            CompanyDB = param["CompanyDB"];
            ApiRemota = param["api remota"];
        }
    }
}