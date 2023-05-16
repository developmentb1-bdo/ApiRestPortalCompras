using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using S7TechIntegracao.API.Utils;
using Swashbuckle.Swagger.Annotations;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class AttachmentsController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetAll")]
        [Route("api/Attachments")]
        public async Task<IHttpActionResult> GetAll()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/Attachments/GetAll] [{sessionId}]");

            try
            {
                var ret = AttachmentsObj.GetInstance().ConsultarTodos();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/Attachments/GetAll] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/Attachments/GetAll] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByDocEntry")]
        [Route("api/Attachments/{docEntry}")]
        public async Task<IHttpActionResult> GetByDocEntry([FromUri] int docEntry)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/Attachments/docEntry] [{sessionId}]");

            try
            {
                var ret = AttachmentsObj.GetInstance().Consultar(docEntry);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/Attachments/docEntry] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/Attachments/docEntry]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetAttachmentsPath")]
        [Route("api/AttachmentsPath/")]
        public async Task<IHttpActionResult> GetAttachmentsPath()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/AttachmentsPath] [{sessionId}]");

            try
            {
                var ret = AttachmentsObj.GetInstance().ConsultarPastaAnexo();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/AttachmentsPath] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/AttachmentsPath] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("Post")]
        [Route("api/Attachments")]
        public async Task<IHttpActionResult> Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/Attachments/Post] [{sessionId}]");

            try
            {
                var novoPn = AttachmentsObj.GetInstance().Cadastrar(model);

                return Ok(novoPn);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/Attachments/Post] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Inicio -- [api/Attachments/Post] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("PostModel")]
        [Route("api/AttachmentsModel")]
        public async Task<IHttpActionResult> PostModel([FromBody] Attachment model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Log4Net.Log.Info($"Inicio -- [api/AttachmentsModel] [{sessionId}]");

            try
            {
                var novoAnexo = AttachmentsObj.GetInstance().Cadastrar(model);

                return Ok(novoAnexo);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/AttachmentsModel] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/AttachmentsModel] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetFileById")]
        [Route("api/Attachments/File/{absEntry}/{rowNumber}")]
        public async Task<IHttpActionResult> GetFileById([FromUri] int absEntry, int rowNumber)
        {
            try
            {
                Conexao.GetInstance().Login();

                var attachment = AttachmentsObj.GetInstance().Consultar(absEntry);
                var attachmentLine = attachment.Attachments2_Lines[rowNumber];

                return ResponseMessage(CreateResponse(attachmentLine));
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPatch]
        [SwaggerOperation("Update")]
        [Route("api/Attachments2/Update")]
        public IHttpActionResult UpdateAttachments2(int absEntry, object document)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            Utils.Log4Net.Log.Info($"Inicio -- [api/Attachments2/Update] [{sessionId}]");

            try
            {
                AttachmentsObj.GetInstance().UpdateAttachments(absEntry, document);

                return Ok("Aprovado");
            }
            catch (Exception ex)
            {
                Utils.Log4Net.Log.Error($"[api/Attachments2/Update]  [{sessionId}] {ex.Message}");

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
                Utils.Log4Net.Log.Info($"Fim -- [api/Attachments2/Update] [{sessionId}]");
            }
        }


        private HttpResponseMessage CreateResponse(AttachmentsLine attachmentsLine)
        {
            var targetPath = AttachmentsObj.GetInstance().ConsultarPastaAnexo();

            var dataBytes = File.ReadAllBytes($"{targetPath}{attachmentsLine.FileName}.{attachmentsLine.FileExtension}");
            var dataStream = new MemoryStream(dataBytes);

            HttpResponseMessage httpResponseMessage = Request.CreateResponse(HttpStatusCode.OK);
            httpResponseMessage.Content = new StreamContent(dataStream);
            httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            httpResponseMessage.Content.Headers.ContentDisposition.FileName = $"{attachmentsLine.FileName}.{attachmentsLine.FileExtension}";
            httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");

            return httpResponseMessage;
        }
    }
}