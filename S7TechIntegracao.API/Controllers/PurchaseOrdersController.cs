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
    public class PurchaseOrdersController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/PurchaseOrders")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                if (docEntry == -1)
                    return Ok(PurchaseOrdersObj.GetInstance().ConsultarTodos());
                else
                    return Ok(PurchaseOrdersObj.GetInstance().Consultar(docEntry));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocNum")]
        [Route("api/PurchaseOrders({docNum})")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int docNum)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseOrdersObj.GetInstance().ConsultarPorDocNum(docNum);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByRequisitante")]
        [Route("api/PurchaseOrders/ConsultaPorRequisitante")]     //Antigo TipoUsuário 1  -- precisará ser paginado
        public async Task<IHttpActionResult> GetByRequisitante([FromUri] int requisitante, [FromUri] int pagina)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseOrdersObj.GetInstance().ConsultarPorRequisitante(requisitante, pagina);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByRequisitanteStatus")]
        [Route("api/PurchaseOrders/ConsultaPorRequisitanteStatus")]    
        public async Task<IHttpActionResult> GetByRequisitanteStatus([FromUri] int requisitante, [FromUri] int docEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseOrdersObj.GetInstance().ConsultarPorRequisitanteStatus(requisitante, docEntry);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }



        [HttpGet]
        [SwaggerOperation("GetByAprovador")]
        [Route("api/PurchaseOrders/ConsultaPorAprovador")]     //Antigo TipoUsuário 2
        public async Task<IHttpActionResult> GetByAprovador([FromUri] int requisitante,  [FromUri] DateTime dataDe, [FromUri] DateTime dataAte)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseOrdersObj.GetInstance().ConsultarPorAprovador(requisitante, dataDe, dataAte);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("ConsultarPedidosComprasPendentesAprovacao")]
        [Route("api/PurchaseOrders/ConsultarPedidosPendentesAprovacao/{empId}")]
        public async Task<IHttpActionResult> ConsultarPedidosPendentesAprovacao([FromUri] int empId)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseOrdersObj.GetInstance().ConsultarPedidosComprasPendentesAprovacao(empId);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("ConsultarPedidosComprasPendentesAprovacaoPorValor")]
        [Route("api/PurchaseOrders/ConsultarPedidosPendentesAprovacaoPorValor/{empId}")]
        public async Task<IHttpActionResult> ConsultarPedidosPendentesAprovacaoPorValor([FromUri] int empId)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseOrdersObj.GetInstance().ConsultarPedidosComprasPendentesAprovacaoPorValor(empId);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }


        [HttpGet]
        [SwaggerOperation("ConsultarDraftKeyPedidoCompras")]
        [Route("api/PurchaseOrders/ConsultarDraftKeyPedidoCompras/{docEntry}")]
        public async Task<IHttpActionResult> ConsultarDraftKeyPedidoCompras([FromUri] int docEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseOrdersObj.GetInstance().ConsultarDraftKey(docEntry);

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
        [Route("api/PurchaseOrders")]
        public IHttpActionResult Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = PurchaseOrdersObj.GetInstance().Cadastrar(model);

                if (novoPn == null)
                    return Ok();

                return Ok(novoPn);
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
                    return Content(HttpStatusCode.BadRequest, e.Message);
                }
            }
        }

        [HttpPost]
        [SwaggerOperation("PostModel")]
        [Route("api/PurchaseOrdersModel")]
        public async Task<IHttpActionResult> PostModel([FromBody] Documents model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = PurchaseOrdersObj.GetInstance().Cadastrar(model);

                return Ok(novoPn);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }


        [HttpPatch]
        [SwaggerOperation("Update")]
        [Route("api/PurchaseOrder/Update")]
        public IHttpActionResult UpDatePurchaseOrder(int docEntry, object document)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Utils.Log4Net.Log.Info($"Inicio -- [api/PurchaseOrder/Update] [{sessionId}]");

            try
            {
                PurchaseOrdersObj.GetInstance().UpdatePurchaseOrder(docEntry, document);

                return Ok("Atualizado");
            }
            catch (Exception ex)
            {
                Utils.Log4Net.Log.Error($"[api/PurchaseOrder/Update]  [{sessionId}] {ex.Message}");

                try
                {
                    var jsonObj = JObject.Parse(ex.Message);
                    return Content(HttpStatusCode.BadRequest, jsonObj);
                }
                catch (Exception e)
                {
                    return Content(HttpStatusCode.BadRequest, e.Message);
                }
            }
            finally
            {
                Utils.Log4Net.Log.Info($"Fim -- [api/PurchaseOrder/Update] [{sessionId}]");
            }
        }
    }
}