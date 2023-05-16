using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class BPPaymentMethod
    {
        public string PaymentMethodCode { get; set; }
        public int RowNumber { get; set; }
        public string BPCode { get; set; }
    }
}