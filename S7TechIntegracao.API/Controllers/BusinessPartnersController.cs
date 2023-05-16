using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using S7TechIntegracao.API.Utils;
using Swashbuckle.Swagger.Annotations;

namespace S7TechIntegracao.API.Controllers
{
    public class BusinessPartnersController : ApiController
    {
        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/BusinessPartners")]
        public IHttpActionResult GetByCardCode([FromUri] string cardCode = "")
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/Get] [{sessionId}]");

                var ret = new List<BusinessPartners>();

                if (string.IsNullOrEmpty(cardCode))
                    ret = BusinessPartnersObj.GetInstance().ConsultarTodos();
                else
                    ret.Add(BusinessPartnersObj.GetInstance().Consultar(cardCode));

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/Get] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/Get] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/BusinessPartners/GetDateTime")]
        public IHttpActionResult GetByDateTime([FromUri] string date = "", int skip = 0, string operacao = "C")
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/GetDateTime] [{sessionId}]");

                var ret = new List<BusinessPartners>();

                ret = BusinessPartnersObj.GetInstance().ConsultarDateTime( date, skip, operacao);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/GetDateTime] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/GetDateTime] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/BusinessPartners/PesquisarCardCode")]
        public IHttpActionResult GetByContainsCardCode([FromUri] string cardCode)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/PesquisarCardCode] [{sessionId}]");

                var ret = BusinessPartnersObj.GetInstance().ConsultarCardCode(cardCode);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/PesquisarCardCode] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/PesquisarCardCode] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/BusinessPartners/PesquisarCardName")]
        public IHttpActionResult GetByContainsCardName([FromUri] string cardName)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/PesquisarCardName] [{sessionId}]");

                var ret = BusinessPartnersObj.GetInstance().ConsultarCardName(cardName);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/PesquisarCardName] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/PesquisarCardName] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByTaxId")]
        [Route("api/BusinessPartnersGetByTaxId")]
        public IHttpActionResult GetByTaxId([FromUri] string taxId, BusinessPartnersTaxIdOption option, BusinessPartnersTypeOption tipo)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartnersGetByTaxId] [{sessionId}]");

                var ret = BusinessPartnersObj.GetInstance().ConsultarByTaxId(taxId, option, tipo);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartnersGetByTaxId] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartnersGetByTaxId] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetClientes")]
        [Route("api/BusinessPartners/ConsultarClientes")]
        public IHttpActionResult GetClientes()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/ConsultarClientes] [{sessionId}]");

                var ret = BusinessPartnersObj.GetInstance().ConsultarClientes();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/ConsultarClientes] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/ConsultarClientes] [{sessionId}]");
            }
        }

        [HttpGet]
        [SwaggerOperation("GetFornecedores")]
        [Route("api/BusinessPartners/ConsultarFornecedores")]
        public IHttpActionResult GetFornecedores()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/ConsultarFornecedores] [{sessionId}]");

                var ret = BusinessPartnersObj.GetInstance().ConsultarFornecedores();

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/ConsultarFornecedores] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/ConsultarFornecedores] [{sessionId}]");
            }
        }

        //[HttpGet]
        //[SwaggerOperation("GetFornecedoresPortal")]
        //[Route("api/BusinessPartners/ConsultarFornecedoresPortal")]
        //public IHttpActionResult GetFornecedoresPortal()
        //{
        //    Conexao.GetInstance().Login();
        //    var sessionId = Conexao.GetInstance().SessionId;

        //    try
        //    {
        //        log.Info($"Inicio -- [api/BusinessPartners/ConsultarFornecedoresPortal] [{sessionId}]");

        //        var ret = BusinessPartnersObj.GetInstance().ConsultarFornecedoresPortal();

        //        return Ok(ret);
        //    }
        //    catch (Exception ex)
        //    {
        //        log.Error($"[api/BusinessPartners/ConsultarFornecedoresPortal] [{sessionId}] {ex.Message}");

        //        var jsonObj = JObject.Parse(ex.Message);
        //        return Content(HttpStatusCode.BadRequest, jsonObj);
        //    }
        //    finally
        //    {
        //        log.Info($"Fim -- [api/BusinessPartners/ConsultarFornecedoresPortal] [{sessionId}]");
        //    }
        //}

        [HttpPost]
        [SwaggerOperation("PostModel")]
        [Route("api/BusinessPartnersModel")]
        public IHttpActionResult PostModel([FromBody]BusinessPartners model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try 
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartnersModel] [{sessionId}]");

                var novoPn = BusinessPartnersObj.GetInstance().Cadastrar(model);

                return Ok(novoPn);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartnersModel] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartnersModel] [{sessionId}]");
            }
        }

        [HttpPost]
        [SwaggerOperation("Post")]
        [Route("api/BusinessPartners")]
        public IHttpActionResult Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/Post] [{sessionId}]");

                var novoPn = BusinessPartnersObj.GetInstance().Cadastrar(model);

                return Ok(novoPn);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/Post] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/Post] [{sessionId}]");
            }
        }

        [HttpPatch]
        [SwaggerOperation("Patch")]
        [Route("api/BusinessPartners")]
        public IHttpActionResult Patch([FromUri] string cardCode, [FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                Log4Net.Log.Info($"Inicio -- [api/BusinessPartners/Patch] [{sessionId}]");

                var ret = BusinessPartnersObj.GetInstance().Atualizar(cardCode, model);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[api/BusinessPartners/Patch] [{sessionId}] {ex.Message}");

                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
            finally
            {
                Log4Net.Log.Info($"Fim -- [api/BusinessPartners/Patch] [{sessionId}]");
            }
        }
    }
}