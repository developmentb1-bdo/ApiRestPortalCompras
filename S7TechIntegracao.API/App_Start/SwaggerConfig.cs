using System.Web.Http;
using WebActivatorEx;
using Swashbuckle.Application;
using System;
using S7TechIntegracao.API;
using System.Net.Http;
using Swashbuckle.Swagger;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System.IO;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace S7TechIntegracao.API
{
    public static class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            var path = Path.Combine(AppContext.BaseDirectory, "bin/S7TechIntegracao.API.xml");

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {


                    c.RootUrl(req => req.ComputeHostAsSeenByOriginalClient());

                    c.SingleApiVersion("v1", "Web API BdoIntegracao");

                    c.PrettyPrint();

                    c.SchemaFilter<SwaggerExcludeFilter>();


                })
                .EnableSwaggerUi(c =>
                {

                    c.DisableValidator();


                    c.EnableApiKeySupport("SessionId", "header");
                });
        }
    }
}
