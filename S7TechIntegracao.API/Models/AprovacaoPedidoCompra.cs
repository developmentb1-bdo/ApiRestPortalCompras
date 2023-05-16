using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class AprovacaoPedidoCompra
    {
        public int DraftEntry { get; set; }
        public int EmpId { get; set; }
        public string Mensagem { get; set; }
        public string CentroCusto { get; set; }

        public AprovacaoPedidoCompra()
        {

        }
    }
}