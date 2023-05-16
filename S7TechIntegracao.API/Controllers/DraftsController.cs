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
    public class DraftsController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/Drafts")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                return Ok(DraftsObj.GetInstance().Consultar(docEntry));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetUltimoDocEntry")]
        [Route("api/ConsultarUltimoDocEntry/{solicitante}")]
        public async Task<IHttpActionResult> GetByUltimoDocEntry([FromUri] int solicitante)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                return Ok(DraftsObj.GetInstance().ConsultarUltimoDocEntry(solicitante));
            }
            catch (Exception ex)
            {
                try
                {
                    var jsonObj = JObject.Parse(ex.Message);
                    return Content(HttpStatusCode.BadRequest, jsonObj);
                }
                catch
                {
                    return Content(HttpStatusCode.BadRequest, ex.Message);
                }
            }
        }

        [HttpGet]
        [SwaggerOperation("GetEsbocoGui")]
        [Route("api/Drafts/ConsultarPorGuid/{guid}")]
        public async Task<IHttpActionResult> GetByEsbocoGuid([FromUri] string guid)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                return Ok(DraftsObj.GetInstance().ConsultarPorGuid(guid));
            }
            catch (Exception ex)
            {
                try
                {
                    var jsonObj = JObject.Parse(ex.Message);
                    return Content(HttpStatusCode.BadRequest, jsonObj);
                }
                catch
                {
                    return Content(HttpStatusCode.BadRequest, ex.Message);
                }
            }
        }

        [HttpPatch]
        [SwaggerOperation("AdicionarEsbocoAprovado")]
        [Route("api/Drafts/AdicionarEsbocoAprovado")]
        public async Task<IHttpActionResult> AdicionarEsbocoAprovado([FromUri] int docEntry)
        {
            //Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                DraftsObj.GetInstance().AdicionarEsbocoAprovado(docEntry);

                return Ok();
            }
            catch (Exception ex)
            {
                try
                {
                    var jsonObj = JObject.Parse(ex.Message);
                    return Content(HttpStatusCode.BadRequest, jsonObj);
                }
                catch (Exception e)
                {
                    return Content(HttpStatusCode.BadRequest, ex.Message);
                }
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocNum")]
        [Route("api/Drafts/DocNum/{docNum}")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int docNum)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                return Ok(DraftsObj.GetInstance().ConsultarPorDocNum(docNum));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }
    }
}