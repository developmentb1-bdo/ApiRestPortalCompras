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
    public class ProfitCentersController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/ProfitCenters")]
        public async Task<IHttpActionResult> GetProfitCenters([FromUri] string code = "")
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                if (string.IsNullOrEmpty(code))
                    return Ok(ProfitCentersObj.GetInstance().ConsultarTodos());
                else
                    return Ok(ProfitCentersObj.GetInstance().Consultar(code));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByContainsCode")]
        [Route("api/ProfitCenters/PesquisarCode")]
        public async Task<IHttpActionResult> GetByContainsCode([FromUri] string code = "", int top = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = ProfitCentersObj.GetInstance().ConsultarPorCode(code, top);

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
