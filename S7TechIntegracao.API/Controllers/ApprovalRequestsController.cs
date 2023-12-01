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
    public class ApprovalRequestsController : ApiController
    {

        [HttpPost]
        [SwaggerOperation("RegraAprovacaoInterna")]
        [Route("api/ApprovalRequests/AdicionarRegraAprovacaoInterna/{draftEntry}")]
        public IHttpActionResult RegraAprovacaoInterna([FromUri] int draftEntry, [FromBody] ParametrosRegraAprovacaoInterna parametros)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/AdicionarRegraAprovacaoInterna/draftEntry] [{sessionId}]");

            try
            {
                var ret = ApprovalRequestsObj.GetInstance().AdicionarRegraAprovacaoInterna(draftEntry);

                //if (parametros?.EnviarEmail ?? false)
                if (ret.U_ObjType != "1470000113")
                {
                    PurchaseOrdersObj.GetInstance().EnviarEmail(ret.Code);
                }
                else
                {
                    PurchaseRequestsObj.GetInstance().EnviarEmail(ret.Code);
                }

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/AdicionarRegraAprovacaoInterna/draftEntry] DraftEntry:{draftEntry} \r\n [{sessionId}] \r\n {ex.Message}");              

               // throw ex;

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/AdicionarRegraAprovacaoInterna/draftEntry] DraftEntry:{draftEntry} \r\n [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("AprovarSolicitacaoCompras")]
        [Route("api/ApprovalRequests/AprovarSolicitacaoCompras")]
        public IHttpActionResult AprovarSolicitacaoCompras([FromBody] AprovacaoSolicitacaoCompra aprovacao)
        {
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/AprovarSolicitacaoCompras] [{sessionId}]");

            try
            {
                PurchaseRequestsObj.GetInstance().AprovarReprovarSolicitacaoPendenteAprovacao(aprovacao, "Y");

                return Ok();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/AprovarSolicitacaoCompras] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/AprovarSolicitacaoCompras] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("ReprovarSolicitacaoCompras")]
        [Route("api/ApprovalRequests/ReprovarSolicitacaoCompras")]
        public IHttpActionResult ReprovarSolicitacaoCompras([FromBody] AprovacaoSolicitacaoCompra aprovacao)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/ReprovarSolicitacaoCompras] [{sessionId}]");

            try
            {
                PurchaseRequestsObj.GetInstance().AprovarReprovarSolicitacaoPendenteAprovacao(aprovacao, "N");

                return Ok();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/ReprovarSolicitacaoCompras] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/ReprovarSolicitacaoCompras] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("AprovarPedidoCompras")]
        [Route("api/ApprovalRequests/AprovarPedidoCompras")]
        public IHttpActionResult AprovarPedidoCompras([FromBody] AprovacaoPedidoCompra aprovacao)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;          

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/AprovarPedidoCompras] [{sessionId}]");

            try
            {
                PurchaseOrdersObj.GetInstance().AprovarReprovarPedidoPendenteAprovacao(aprovacao, "Y");

                return Ok();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/AprovarPedidoCompras] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/AprovarPedidoCompras] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("ReprovarPedidoCompras")]
        [Route("api/ApprovalRequests/ReprovarPedidoCompras")]
        public IHttpActionResult ReprovarPedidoCompras([FromBody] AprovacaoPedidoCompra aprovacao)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/ReprovarPedidoCompras] [{sessionId}]");

            try
            {
                PurchaseOrdersObj.GetInstance().AprovarReprovarPedidoPendenteAprovacao(aprovacao, "N");

                return Ok();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/ReprovarPedidoCompras] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/ReprovarPedidoCompras] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("AprovarPedidoComprasPorValor")]
        [Route("api/ApprovalRequests/AprovarPedidoComprasPorValor")]
        public IHttpActionResult AprovarPedidoComprasPorValor([FromBody] AprovacaoPedidoCompraPorValor aprovacao)
        {
            //Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/AprovarPedidoComprasPorValor] [{sessionId}]");

            try
            {
                PurchaseOrdersObj.GetInstance().AprovarReprovarPedidoComprasPorValor(aprovacao, "Y");

                return Ok();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/AprovarPedidoComprasPorValor] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/AprovarPedidoComprasAlcadaValor] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("ReprovarPedidoComprasPorValor")]
        [Route("api/ApprovalRequests/ReprovarPedidoComprasPorValor")]
        public IHttpActionResult ReprovarPedidoComprasPorValor([FromBody] AprovacaoPedidoCompraPorValor aprovacao)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/ReprovarPedidoComprasPorValor] [{sessionId}]");

            try
            {
                PurchaseOrdersObj.GetInstance().AprovarReprovarPedidoComprasPorValor(aprovacao, "N");

                return Ok();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/ReprovarPedidoComprasPorValor] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/ReprovarPedidoComprasPorValor] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("AprovarPedidoComprasPorEmail")]
        [Route("api/ApprovalRequests/AprovarPedidoComprasEmail/{draftEntry}/{empID}")]
        public IHttpActionResult AprovarPedidoComprasEmail([FromUri] int draftEntry, [FromUri] int empID)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/AprovarPedidoComprasEmail/draftEntry/empID] [{sessionId}]");

            try
            {
                var aprovacao = new AprovacaoPedidoCompra()
                {
                    DraftEntry = draftEntry,
                    EmpId = empID,
                    Mensagem = string.Empty
                };

                PurchaseOrdersObj.GetInstance().AprovarReprovarPedidoPendenteAprovacao(aprovacao, "Y");

                return Ok("Pedido de compras - Aprovado");
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/AprovarPedidoComprasEmail/draftEntry/empID] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/AprovarPedidoComprasEmail/draftEntry/empID] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("ReprovarPedidoComprasPorEmail")]
        [Route("api/ApprovalRequests/ReprovarPedidoComprasEmail/{draftEntry}/{empID}")]
        public IHttpActionResult ReprovarPedidoComprasEmail([FromUri] int draftEntry, [FromUri] int empID)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/ReprovarPedidoComprasEmail/draftEntry/empID] [{sessionId}]");

            try
            {
                var aprovacao = new AprovacaoPedidoCompra()
                {
                    DraftEntry = draftEntry,
                    EmpId = empID,
                    Mensagem = string.Empty
                };

                PurchaseOrdersObj.GetInstance().AprovarReprovarPedidoPendenteAprovacao(aprovacao, "N");

                return Ok("Pedido de compras - Reprovado");
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/ReprovarPedidoComprasEmail/draftEntry/empID] [{sessionId}] {ex.Message}");

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
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/ReprovarPedidoComprasEmail/draftEntry/empID] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetEmailGestorArea")]
        [Route("api/ApprovalRequests/ConsultarEmailGestorArea")]
        public IHttpActionResult GetEmailGestorArea(string centroCusto)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/ConsultarEmailGestorArea] [{sessionId}]");

            try
            {
                return Ok(ApprovalRequestsObj.GetInstance().ConsultarEmailGestorArea(centroCusto));
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/ConsultarEmailGestorArea] [{sessionId}] {ex.Message}");

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
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/ConsultarEmailGestorArea] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetStatusAprovadoresEsboco")]
        [Route("api/ApprovalRequests/ConsultarStatusAprovadoresEsboco/{draftEntry}")]
        public IHttpActionResult GetStatusAprovadoresEsboco([FromUri] int draftEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/ConsultarStatusAprovadoresEsboco/draftEntry] [{sessionId}]");

            try
            {
                return Ok(ApprovalRequestsObj.GetInstance().ConsultarStatusAprovadoresEsboco(draftEntry));
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/ConsultarStatusAprovadoresEsboco/draftEntry] [{sessionId}] {ex.Message}");

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
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/ConsultarStatusAprovadoresEsboco/draftEntry] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetRuleApproval")]
        [Route("api/ApprovalRequests/ConsultarRegraAprovacao")]
        public IHttpActionResult GetRuleApproval([FromUri] double totalDaLinha, string centroCusto,string obj)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/ApprovalRequests/ConsultarRegraAprovacao] [{sessionId}]");

            try
            {
                return Ok(ApprovalRequestsObj.GetInstance().ConsultarRegraAprovacao(totalDaLinha, centroCusto,obj));
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/ApprovalRequests/ConsultarRegraAprovacao] [{sessionId}] {ex.Message}");

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
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/ApprovalRequests/ConsultarRegraAprovacao] [{sessionId}]");
            }
        }
    }
}