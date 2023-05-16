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
    /// <summary>
    /// Recebimento de Mercadoria
    /// </summary>
    public class InventoryGenExitsController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/InventoryGenExits")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                if (docEntry == -1)
                    return Ok(InventoryGenExitsObj.GetInstance().ConsultarTodos());
                else
                    return Ok(InventoryGenExitsObj.GetInstance().Consultar(docEntry));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocNum")]
        [Route("api/InventoryGenExits({docNum})")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int docNum)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = InventoryGenExitsObj.GetInstance().ConsultarPorDocNum(docNum);

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
        [Route("api/InventoryGenExits")]
        public async Task<IHttpActionResult> Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = InventoryGenExitsObj.GetInstance().Cadastrar(model);

                return Ok(novoPn);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpPost]
        [SwaggerOperation("PostModel")]
        [Route("api/InventoryGenExitsModel")]
        public async Task<IHttpActionResult> PostModel([FromBody] Documents model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoDocumento = InventoryGenExitsObj.GetInstance().Cadastrar(model);

                return Ok(novoDocumento);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }
    }
}