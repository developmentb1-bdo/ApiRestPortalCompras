using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class ProjectsController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("Get")]
        [Route("api/Projects")]
        public async Task<IHttpActionResult> GetProjects([FromUri] string code = "")
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = new List<Project>();

                if (string.IsNullOrEmpty(code))
                    ret = ProjectsObj.GetInstance().ConsultarTodos();
                else
                    ret.Add(ProjectsObj.GetInstance().Consultar(code));

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByContainsCode")]
        [Route("api/Projects/PesquisarCode")]
        public async Task<IHttpActionResult> GetByContainsCode([FromUri] string code = "", int top = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = ProjectsObj.GetInstance().ConsultarPorCode(code, top);

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
