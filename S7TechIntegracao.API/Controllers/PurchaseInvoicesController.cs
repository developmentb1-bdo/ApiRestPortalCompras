using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class PurchaseInvoicesController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/PurchaseInvoices")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                if (docEntry == -1)
                    return Ok(PurchaseInvoicesObj.GetInstance().ConsultarTodos());
                else
                    return Ok(PurchaseInvoicesObj.GetInstance().Consultar(docEntry));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocNum")]
        [Route("api/PurchaseInvoices({docNum})")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int docNum)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseInvoicesObj.GetInstance().ConsultarPorDocNum(docNum);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByNumAtCard")]
        [Route("api/PurchaseInvoices/ConsultarNumAtCard")]
        public async Task<IHttpActionResult> GetByNumAtCard([FromUri] string numAtCard)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseInvoicesObj.GetInstance().ConsultarPorNumAtCard(numAtCard);

                if (ret.Count == 0)
                    return NotFound();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpPost]
        [SwaggerOperation("Post")]
        [Route("api/PurchaseInvoices")]
        public async Task<IHttpActionResult> Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var documento = PurchaseInvoicesObj.GetInstance().Cadastrar(model);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpPost]
        [SwaggerOperation("PostModel")]
        [Route("api/PurchaseInvoicesModel")]
        public async Task<IHttpActionResult> PostModel([FromBody]Documents model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var documento = PurchaseInvoicesObj.GetInstance().Cadastrar(model);

                return Ok(documento);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }
    }
}