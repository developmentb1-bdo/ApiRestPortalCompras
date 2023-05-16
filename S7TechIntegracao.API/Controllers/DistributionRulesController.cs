using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using S7TechIntegracao.API.Utils;
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
    public class DistributionRulesController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/DistributionRules")]
        public async Task<IHttpActionResult> GetDistributionRules([FromUri] string factorCode = "")
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = new List<DistributionRule>();

                if (string.IsNullOrEmpty(factorCode))
                    ret = DistributionRulesObj.GetInstance().ConsultarTodos();
                else
                    ret.Add(DistributionRulesObj.GetInstance().Consultar(factorCode));

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetDistributionRulesByDimension")]
        [Route("api/DistributionRules/PesquisarPorCode")]
        public async Task<IHttpActionResult> GetDistributionRulesByDimension([FromUri] string code = "", int top = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = DistributionRulesObj.GetInstance().ConsultarPorCode(code, top);

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
