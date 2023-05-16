using S7TechIntegracao.API.App_Start;
using System.Net;
using System.Net.Security;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace S7TechIntegracao.API
{
    public class WebApiApplication : HttpApplication {
        protected void Application_Start() {

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            SapDIConfig.GetConfigurationInitial();

            //Change SSL checks so that all checks pass
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });
        }
    }
}
