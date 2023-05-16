using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
namespace S7TechIntegracao.API.Controllers
{
    public class JournalVoucherServiceController : ApiController
    {
        [HttpPost]
        [SwaggerOperation("Post")]
        [Route("api/PostJournalVoucherService")]
        public IHttpActionResult Post([FromBody] JournalVouchersService_Add model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var journalentry = JournalVoucherServiceObj.GetInstance().Cadastrar(model);

                if (journalentry == null)
                    return Ok();

                return Ok(journalentry);
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

        [HttpGet]
        [SwaggerOperation("GetAllJournalEntry")]
        [Route("api/GetAllJournalEntry")]
        public async Task<IHttpActionResult> GetAllJournalEntry()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                
                var ret = JournalVoucherServiceObj.GetInstance().ConsultarTodasTransacoesContabeis();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }


        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/GetJournalEntry({transId})")]
        public async Task<IHttpActionResult> GetByDocNum([FromUri] int transId)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = JournalVoucherServiceObj.GetInstance().ConsultarTransacaoContabil(transId);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }


        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/FunctionalAreaTrigger")]
        public async Task<IHttpActionResult> GetFunctionalAreaTrigger([FromUri] string costingCode2)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = JournalVoucherServiceObj.GetInstance().ConsultarGatilhoArea(costingCode2);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

               
        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/SegmentTrigger")]
        public async Task<IHttpActionResult> GetSegmentTrigger([FromUri] string projectCode)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = JournalVoucherServiceObj.GetInstance().ConsultarGatilhoSegmento(projectCode);

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