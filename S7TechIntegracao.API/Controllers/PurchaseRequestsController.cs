using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using S7TechIntegracao.API.Utils;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class PurchaseRequestsController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/PurchaseRequests")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                if (docEntry == -1)
                    return Ok(PurchaseRequestsObj.GetInstance().ConsultarTodos());
                else
                    return Ok(PurchaseRequestsObj.GetInstance().Consultar(docEntry));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocNum")]
        [Route("api/PurchaseRequests({docNum})")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int docNum)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseRequestsObj.GetInstance().ConsultarPorDocNum(docNum);

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
        [Route("api/PurchaseRequests/ConsultaPorRequisitante")]
        public async Task<IHttpActionResult> GetByRequisitante([FromUri] string solicitante, [FromUri] int pagina)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseRequestsObj.GetInstance().ConsultarPorRequisitante(solicitante, pagina);

                return Ok(ret);
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
        [SwaggerOperation("GetByRequisitanteStatus")]
        [Route("api/PurchaseRequests/ConsultaPorRequisitanteStatus")]
        public async Task<IHttpActionResult> GetByRequisitanteStatus([FromUri] int requisitante, [FromUri] int docEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseRequestsObj.GetInstance().ConsultarPorRequisitanteStatus(requisitante, docEntry);

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
        [Route("api/PurchaseRequests/ConsultaPorAprovador")]
        public async Task<IHttpActionResult> GetByAprovador([FromUri] string solicitante, [FromUri] DateTime dataDe, [FromUri] DateTime dataAte)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseRequestsObj.GetInstance().ConsultarAprovador(solicitante, dataDe, dataAte);

                return Ok(ret);
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

        /// <summary>
        /// Consulta solicitações de compras que foram gravadas pelo processo de aprovação e estão em esboço aguardando ação do aprovador
        /// </summary>
        /// <param name="gestorArea"></param>
        /// <returns>Lista de documento dos seguintes tipos
        /// Pedido de compra 			- oPurchaseOrders 22 
        /// Solicitação de compra 		- oPurchaseRequest 1470000113
        /// Oferta de compra (Cotação)	- oPurchaseQuotations 540000006 
        /// </returns>
        [HttpGet]
        [SwaggerOperation("ConsultaSolicitacoesPendentesAprovacao")]
        [Route("api/PurchaseRequests/ConsultaSolicitacoesPendentesAprovacao/{gestorArea}")]
        public async Task<IHttpActionResult> ConsultaSolicitacoesPendentesAprovacao([FromUri] string gestorArea)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = PurchaseRequestsObj.GetInstance().ConsultarSolicitacoesComprasPendentesAprovacao(gestorArea);

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
        [Route("api/PurchaseRequests")]
        public async Task<IHttpActionResult> Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = PurchaseRequestsObj.GetInstance().Cadastrar(model);

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
        [Route("api/PurchaseRequestsModel")]
        public async Task<IHttpActionResult> PostModel([FromBody] Documents model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = PurchaseRequestsObj.GetInstance().Cadastrar(model);

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
        [Route("api/PurchaseRequestUpdate/Update")]
        public IHttpActionResult UpdatePurchaseRequest(int docEntry, object document)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Utils.Log4Net.Log.Info($"Inicio -- [api/PurchaseRequestUpdate/Update] [{sessionId}]");

            try
            {
                PurchaseRequestsObj.GetInstance().UpdatePurchaseRequest(docEntry, document);

                string ret = "Atualizado";

                return Ok(ret);
               

            }
            catch (Exception ex)
            {
                Utils.Log4Net.Log.Error($"[api/PurchaseRequestUpdate/Update]  [{sessionId}] {ex.Message}");

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
                Utils.Log4Net.Log.Info($"Fim -- [api/PurchaseRequestUpdate/Update] [{sessionId}]");
            }
        }
    }
}