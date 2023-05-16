using S7TechIntegracao.API.Objetos;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Net;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class SBOBobServiceController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/SBOBobServices/GetCurrencies")]
        public IHttpActionResult GetCurrencies()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SBOBobServiceObj.GetInstance().GetCurrencies();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/SBOBobServices/GetSystemCurrency")]
        public IHttpActionResult GetSystemCurrency()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SBOBobServiceObj.GetInstance().GetSystemCurrency();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/SBOBobServices/GetCurrencyRate")]
        public IHttpActionResult GetCurrencyRate(string currency, DateTime date)
        {
            Conexao.GetInstance().Login();

            try
            {
                var ret = SBOBobServiceObj.GetInstance().GetCurrencyRate(currency, date);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }



        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/SBOBobServices/GetPaymentMethods")]
        public IHttpActionResult GetPaymentMethods()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = SBOBobServiceObj.GetInstance().GetPaymentMethods();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}