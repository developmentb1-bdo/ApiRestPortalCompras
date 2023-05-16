using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class UsagesController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/Usages")]
        public async Task<IHttpActionResult> GetUsages()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                return Ok(UsagesObj.GetInstance().ConsultarTodos());
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }
    }
}
