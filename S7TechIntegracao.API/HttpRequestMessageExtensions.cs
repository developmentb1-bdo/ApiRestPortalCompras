using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace S7TechIntegracao.API
{
    public static class HttpRequestMessageExtensions
    {
        public static string ComputeHostAsSeenByOriginalClient(this HttpRequestMessage req)
        {
            var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");

            var authority = req.RequestUri.Authority;               
            var scheme = req.RequestUri.Scheme;
            var port = req.RequestUri.Port;
            var baseUrl = req.GetRequestContext().VirtualPathRoot.TrimStart('/');
            //var baseUrl = req.RequestUri.AbsolutePath.Replace("/swagger/ui/index.html", "");
            //baseUrl = req.RequestUri.AbsolutePath.Replace("/swagger/ui/index", "");
            var portaHttp = Convert.ToInt32(param["PortaHttp"]);
            var portaHttps = Convert.ToInt32(param["PortaHttps"]);

            string uriBase;

#if DEBUG == false

            if (port == 80)
                port = portaHttps;
            else
                port = portaHttp;
#endif

#if DEBUG != false

            uriBase = scheme + "://" + authority + baseUrl;
#else
            uriBase = scheme + "://" + authority + ":" + port.ToString() + "/" + baseUrl;
#endif


            //uriBase = scheme + "://" + authority + ":" + port.ToString() + "/" + baseUrl;
            //var uriBase = scheme + "://" + authority + baseUrl;

            return uriBase;
        }
    }
}