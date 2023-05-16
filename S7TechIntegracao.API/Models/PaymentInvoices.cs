using System;

namespace S7TechIntegracao.API.Models
{
    public class PaymentInvoices
    {
        public int? LineNum { get; set; }
        public int? DocEntry { get; set; }
        public double SumApplied { get; set; }
        public int? DocLine { get; set; }
        public string InvoiceType { get; set; }
        public double? DiscountPercent { get; set; }
        public double PaidSum { get; set; }
        public string InstallmentId { get; set; }
        public double? TotalDiscount { get; set; }
        public string FederalTaxID { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string Canceled { get; set; }
        public string InsTotal { get; set; }
        public string PaidToDate { get; set; }
        public double? Saldo { get; set; }
        public PaymentInvoices()
        {
            InvoiceType = "it_Invoice";
        }

    }
}