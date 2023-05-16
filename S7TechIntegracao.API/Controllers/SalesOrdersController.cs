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
    public class SalesOrdersController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/SalesOrders")]
        public async Task<IHttpActionResult> GetAll()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SalesOrdersObj.GetInstance().ConsultarTodos();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/SalesOrders/DocEntry/{docEntry}")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SalesOrdersObj.GetInstance().Consultar(docEntry);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocNum")]
        [Route("api/SalesOrders/DocNum/{docNum}")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int docNum)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SalesOrdersObj.GetInstance().ConsultarPorDocNum(docNum);

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
        [Route("api/SalesOrders")]
        public async Task<IHttpActionResult> Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = SalesOrdersObj.GetInstance().Cadastrar(model);

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
        [Route("api/SalesOrdersModel")]
        public async Task<IHttpActionResult> PostModel([FromBody] Documents model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = SalesOrdersObj.GetInstance().Cadastrar(model);

                return Ok(novoPn);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }
    }
}