using System.Net;
using System.Web.Http;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using S7TechIntegracao.API.Utils;

namespace S7TechIntegracao.API.Controllers {
    public abstract class RestClientApiController : ApiController {
        protected readonly RestClient Client;
        public CookieContainer CookieContainer { get; private set; }

        protected RestClientApiController() {
            //Client.UseNewtonsoftJson();
        }
    }
}