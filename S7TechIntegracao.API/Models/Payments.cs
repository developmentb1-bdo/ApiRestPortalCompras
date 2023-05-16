using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class Payments
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? TaxDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string DocCurrency { get; set; }
        public string LocalCurrency { get; set; }
        public double? DocRate { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Remarks { get; set; }
        public string JournalRemarks { get; set; }
        public string CashAccount { get; set; }
        public double? CashSum { get; set; }
        public string CheckAccount { get; set; }
        public string TransferAccount { get; set; }
        public double? TransferSum { get; set; }
        public DateTime? TransferDate { get; set; }
        public string TransferReference { get; set; }
        public string BankCode { get; set; }
        public string BankAccount { get; set; }
        public string DocTypte { get; set; }
        public int? BPLID { get; set; }
        public string BPLName { get; set; }
        public List<PaymentInvoice> PaymentInvoices { get; set; }

        public Payments()
        {
            DocType = "rCustomer";
            DocTypte = "rCustomer";
        }
    }
}