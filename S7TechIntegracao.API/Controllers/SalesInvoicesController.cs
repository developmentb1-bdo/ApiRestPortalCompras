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
    public class SalesInvoicesController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/SalesInvoices")]
        public async Task<IHttpActionResult> GetAll()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                return Ok(SalesInvoicesObj.GetInstance().ConsultarTodos());
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }


        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/SalesInvoices/DocEntry/{docEntry}")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
               return Ok(SalesInvoicesObj.GetInstance().Consultar(docEntry));
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocNum")]
        [Route("api/SalesInvoices/DocNum/{docNum}")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int docNum)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SalesInvoicesObj.GetInstance().ConsultarPorDocNum(docNum);

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
        [Route("api/SalesInvoices/ConsultarNumAtCard")]
        public async Task<IHttpActionResult> GetByNumAtCard([FromUri] string numAtCard)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SalesInvoicesObj.GetInstance().ConsultarPorNumAtCard(numAtCard);

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

        [HttpGet]
        [SwaggerOperation("GetByOpenTitles")]
        [Route("api/SalesInvoices/ConsultarTitulosAbertos")]
        public IHttpActionResult GetByOpenTitles([FromUri] string date = "", string hour = "", string codigoCliente = "",  int docEntry = 0, int pagina = 1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;
            
            try
            {
                Log4Net.Log.Info($"Inicio -- [api/SalesInvoices/ConsultarTitulosAbertos] [{sessionId}]");

                var ret = new List<Documents>();

                ret = SalesInvoicesObj.GetInstance().ConsultarTitulosAbertos(date, hour,codigoCliente,docEntry,pagina);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/SalesInvoices/ConsultarTitulosAbertos] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/SalesInvoices/ConsultarTitulosAbertos] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByClosedTitles")]
        [Route("api/SalesInvoices/ConsultarTitulosBaixados")]
        public IHttpActionResult GetByClosedTitles([FromUri] string date = "", string hour = "", string codigoCliente = "", int docEntry = 0, int pagina = 1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/SalesInvoices/ConsultarTitulosBaixados] [{sessionId}]");

                var ret = new List<Documents>();

                ret = SalesInvoicesObj.GetInstance().ConsultarTitulosBaixados(date, hour, codigoCliente, docEntry,pagina);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/SalesInvoices/ConsultarTitulosBaixados] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/SalesInvoices/ConsultarTitulosBaixados] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByCanceledOrClosedTitles")]
        [Route("api/SalesInvoices/ConsultarTitulosCanceladosOuFechados")]
        public IHttpActionResult GetByCanceledOrClosedTitles([FromUri] string date = "", string hour = "", string codigoCliente = "", int docEntry = 0, int pagina = 1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/SalesInvoices/ConsultarTitulosCanceladosOuFechados] [{sessionId}]");

                var ret = new List<Documents>();

                ret = SalesInvoicesObj.GetInstance().ConsultarTitulosCanceladosOuFechados(date, hour, codigoCliente, docEntry, pagina);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/SalesInvoices/ConsultarTitulosCanceladosOuFechados] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/SalesInvoices/ConsultarTitulosCanceladosOuFechados] [{sessionId}]");
            }
        }


        [HttpPost]
        [SwaggerOperation("Post")]
        [Route("api/SalesInvoices")]
        public async Task<IHttpActionResult> Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoDocumento = SalesInvoicesObj.GetInstance().Cadastrar(model);

                DocumentsObj.GetInstance().VerificarCriacaoPagamento(novoDocumento);

                return Ok(novoDocumento);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpPost]
        [SwaggerOperation("PostModel")]
        [Route("api/SalesInvoicesModel")]
        public async Task<IHttpActionResult> PostModel([FromBody] Documents model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = SalesInvoicesObj.GetInstance().Cadastrar(model);

                return Ok(novoPn);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpPatch]
        [SwaggerOperation("Patch")]
        [Route("api/SalesInvoices")]
        public async Task<IHttpActionResult> Patch([FromUri] int docEntry, [FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoDocumento = SalesInvoicesObj.GetInstance().Atualizar(docEntry, model);

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