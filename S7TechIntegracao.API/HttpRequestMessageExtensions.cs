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

#if DEBUG == false

            if (port == 80)
                port = portaHttp;
            else
                port = portaHttps;
#endif

            //if (req.Headers.Contains("X-Forwarded-Host"))
            //{
            //    // we are behind a reverse proxy, use the host that was used by the client
            //    var xForwardedHost = req.Headers.GetValues("X-Forwarded-Host").First();

            //    /*
            //     When multiple apache httpd are chained, each proxy append to the header
            //      with a comma (see //https://httpd.apache.org/docs/2.4/mod/mod_proxy.html#x-headers).
            //      so we need to take only the first host because it is the host that was
            //      requested by the original client.
            //      note that other reverse proxies may behave differently but
            //     we are not taking care of them...
            //     */
            //    var firstForwardedHost = xForwardedHost.Split(',')[0];

            //    authority = firstForwardedHost;
            //}

            //if (req.Headers.Contains("X-Forwarded-Proto"))
            //{
            //    /*
            //     now that we have the host, we also need to determine the protocol used by the
            //     original client.
            //     if present, we are using the de facto standard header X-Forwarded-Proto
            //     otherwise, we fallback to http
            //     note that this is extremely brittle, either because the first proxy
            //     can "forget" to set the header or because another proxy can rewrite it...
            //    */
            //    var xForwardedProto = req.Headers.GetValues("X-Forwarded-Proto").First();
            //    if (xForwardedProto.IndexOf(",") != -1)
            //    {
            //        // >hen multiple apache, X-Forwarded-Proto is also multiple ...
            //        xForwardedProto = xForwardedProto.Split(',')[0];
            //    }

            //    scheme = xForwardedProto;
            //}

            //if (req.Headers.Contains("X-Forwarded-Port"))
            //{
            //    var xForwardedPort = req.Headers.GetValues("X-Forwarded-Port").First();
            //    if (xForwardedPort.IndexOf(",") != -1)
            //    {
            //        // When multiple apache, X-Forwarded-Proto is also multiple ...
            //        xForwardedPort = xForwardedPort.Split(',')[0];
            //    }

            //    int.TryParse(xForwardedPort, out port);
            //}

            //// If we have standard scheme + port, leave out the port in the resulting Url.
            //if (("http".Equals(scheme, StringComparison.InvariantCultureIgnoreCase) && port == 80)
            //    || ("https".Equals(scheme, StringComparison.InvariantCultureIgnoreCase) && port == 443))
            //{
            //    return scheme + "://" + authority;
            //}

            var uriBase = scheme + "://" + authority + ":" + port.ToString() + "/" + baseUrl;

            return uriBase;
        }
    }
}