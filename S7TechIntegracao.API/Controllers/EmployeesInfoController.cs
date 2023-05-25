using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using S7TechIntegracao.API.Utils;
using Swashbuckle.Swagger.Annotations;

namespace S7TechIntegracao.API.Controllers
{
    public class EmployeesInfoController : ApiController
    {   

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/EmployeesInfo")]
        public async Task<IHttpActionResult> GetByEmployeeID([FromUri] int employeeID = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {

                var ret = new List<EmployeeInfo>();

                if (employeeID == -1)
                    ret = EmployeesInfoObj.GetInstance().ConsultarTodos();
                else
                    ret.Add(EmployeesInfoObj.GetInstance().Consultar(employeeID));

                return Ok(ret);
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
        [SwaggerOperation("Get")]
        [Route("api/EmployeesInfo/PesquisarUsuario")]
        public async Task<IHttpActionResult> GetByUsuario([FromUri] string usuario, [FromUri] string senha)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {

                var ret = EmployeesInfoObj.GetInstance().ConsultarEmail(usuario, senha);

                return Ok(ret);
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
        [SwaggerOperation("Get")]
        [Route("api/EmployeesInfo/GetPassEncryptedUser")]
        public async Task<IHttpActionResult> GetByPassEncryptedUser([FromUri] string usuario)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {

                var ret = EmployeesInfoObj.GetInstance().GetByPassEncryptedUser(usuario);

                return Ok(ret);
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
        [Route("api/EmployeesInfoModel")]
        public async Task<IHttpActionResult> PostModel([FromBody] EmployeeInfo model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try 
            {
                var novoPn = EmployeesInfoObj.GetInstance().Cadastrar(model);

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
        [SwaggerOperation("Post")]
        [Route("api/EmployeesInfo")]
        public async Task<IHttpActionResult> Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = EmployeesInfoObj.GetInstance().Cadastrar(model);

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

        [HttpPatch]
        [SwaggerOperation("Patch")]
        [Route("api/EmployeesInfo")]
        public async Task<IHttpActionResult> Patch([FromUri] int employeeID, [FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = EmployeesInfoObj.GetInstance().Atualizar(employeeID, model);

                return Ok(ret);
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


        [HttpPatch]
        [SwaggerOperation("Patch")]
        [Route("api/PatchEncryptedPassForUser")]
        public async Task<IHttpActionResult> PatchEncryptedPassForUser()
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                EmployeesInfoObj.GetInstance().AtualizarSenhaParaEncriptadaDoUsuario();
                return Ok("Atualizado");
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

    }
}