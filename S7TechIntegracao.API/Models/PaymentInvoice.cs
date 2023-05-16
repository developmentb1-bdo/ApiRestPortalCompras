using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class PaymentInvoice
    {
        public int? LineNum { get; set; }
        public int? DocEntry { get; set; }
        public double? SumApplied { get; set; }
        public int? DocLine { get; set; }
        public string InvoiceType { get; set; }
        public double? DiscountPercent { get; set; }
        public double? PaidSum { get; set; }
        public int? InstallmentId { get; set; }
        public double? TotalDiscount { get; set; }

        public PaymentInvoice()
        {
            InvoiceType = "it_Invoice";
        }
    }
}