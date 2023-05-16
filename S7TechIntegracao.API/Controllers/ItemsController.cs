using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Objetos;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace S7TechIntegracao.API.Controllers
{
    public class ItemsController : ApiController
    {
        

        [HttpGet]
        [SwaggerOperation("GetByItemCode")]
        [Route("api/Items")]
        public IHttpActionResult GetByItemCode([FromUri] string itemCode = "")
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = new List<Items>();

                if (string.IsNullOrEmpty(itemCode.Trim()))
                    ret = ItemsObj.GetInstance().ConsultarTodos();
                else
                    ret.Add(ItemsObj.GetInstance().Consultar(itemCode));

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByContainsItemCode")]
        [Route("api/Items/PesquisarItemCode")]
        public IHttpActionResult GetByContainsItemCode([FromUri] string itemCode = "", int top = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = ItemsObj.GetInstance().ConsultarPorItemCode(itemCode, top);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByContainsItemName")]
        [Route("api/Items/PesquisarItemName")]
        public IHttpActionResult GetByContainsItemName([FromUri] string itemName, int top = -1)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = ItemsObj.GetInstance().ConsultarPorItemName(itemName, top);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByItemName")]
        [Route("api/Items({itemEstoque}, {itemCompra}, {itemVenda})")]
        public IHttpActionResult GetByType([FromUri] bool itemEstoque, bool itemCompra, bool itemVenda)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var estoque = itemEstoque ? "Y" : "N";
                var compra = itemCompra ? "Y" : "N";
                var venda = itemVenda ? "Y" : "N";

                var ret = ItemsObj.GetInstance().ConsultarPorTipo(estoque, compra, venda);

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
        [Route("api/Items")]
        public IHttpActionResult Post([FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = ItemsObj.GetInstance().Cadastrar(model);

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
        [Route("api/ItemsModel")]
        public IHttpActionResult PostModel([FromBody] Items model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var novoPn = ItemsObj.GetInstance().Cadastrar(model);

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
        [Route("api/Items")]
        public IHttpActionResult Patch([FromUri] string itemCode, [FromBody] object model)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = ItemsObj.GetInstance().Atualizar(itemCode, model);

                return Ok(ret);
            }
            catch (Exception ex)
            {
                var jsonObj = JObject.Parse(ex.Message);
                return Content(HttpStatusCode.BadRequest, jsonObj);
            }
        }

        [HttpGet]
        [SwaggerOperation("GetByConsultarTodosFiltro")]
        [Route("api/Items/ConsultarTodosFiltro")]
        public IHttpActionResult GetByConsultarTodosFiltro([FromUri] string filtro)
        {
            Conexao.GetInstance().Login();
            var sessionId = Conexao.GetInstance().SessionId;

            try
            {
                var ret = ItemsObj.GetInstance().ConsultarTodos(filtro);

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