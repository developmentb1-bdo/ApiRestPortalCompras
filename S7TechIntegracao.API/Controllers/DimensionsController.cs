using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class DimensionsController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/Dimensions")]
        public async Task<IHttpActionResult> GetDimensions([FromUri] int id = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = new List<Dimension>();

                if (id == -1)
                    ret = DimensionsObj.GetInstance().ConsultarTodos();
                else
                    ret.Add(DimensionsObj.GetInstance().Consultar(id));

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }
    }
}
