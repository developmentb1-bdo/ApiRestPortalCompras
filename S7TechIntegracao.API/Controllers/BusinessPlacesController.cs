using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class BusinessPlacesController : ApiController
    {
        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/BusinessPlaces")]
        public async Task<IHttpActionResult> GetBusinessPlaces([FromUri] int id = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = new List<BusinessPlace>();

                if (id == -1)
                    ret = BusinessPlacesObj.GetInstance().ConsultarTodos();
                else
                    ret.Add(BusinessPlacesObj.GetInstance().Consultar(id));

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
            }
        }
    }
}
