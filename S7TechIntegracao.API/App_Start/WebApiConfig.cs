using System.Web.Http;

namespace S7TechIntegracao.API {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            // Rotas da API da Web
            config.MapHttpAttributeRoutes();
        }
    }
}
