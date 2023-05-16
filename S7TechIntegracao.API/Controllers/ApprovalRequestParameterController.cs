using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class ApprovalRequestParameterController : ApiController
    {
        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/ApprovalRequestParameter/GetByCode")]
        public IHttpActionResult GetByCode([FromUri] string code)
        {
            Conexao.GetInstance().Login();

            try
            {
                var ret = ApprovalRequestParameterObj.GetInstance().GetByCode(code);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/ApprovalRequestParameter/GetByEmployeeId")]
        public IHttpActionResult GetByEmployeeId([FromUri] string employeeId)
        {
            Conexao.GetInstance().Login();

            try
            {
                var ret = ApprovalRequestParameterObj.GetInstance().GetByEmployeeId(employeeId);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }


        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/ApprovalRequestParameter/GetByCostCenterCode")]
        public IHttpActionResult GetByCostCenterCode([FromUri] string costCenter)
        {
            Conexao.GetInstance().Login();

            try
            {
                var ret = ApprovalRequestParameterObj.GetInstance().GetByCostCenterCode(costCenter);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPatch]
        [SwaggerOperation("Patch")]
        [Route("api/ApprovalRequestParameter/AlterarRegraAprovacaoInterna")]
        public IHttpActionResult AlterarRegraAprovacaoInterna([FromBody] S7T_APROVOPRQ model)
        {
            Conexao.GetInstance().Login();

            try
            {
                var ret = ApprovalRequestParameterObj.GetInstance().AlterarAlcadaAprovacaoInterna(model);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}