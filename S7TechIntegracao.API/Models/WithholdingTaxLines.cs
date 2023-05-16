using System;
using System.Collections.Generic;

namespace S7TechIntegracao.API.Models
{
    public class WithholdingTaxLines
    {
        public string WTCode { get; set; }
        public double WTAmountSys { get; set; }
        public double WTAmountFC { get; set; }
        public double WTAmount { get; set; }
        public object WithholdingType { get; set; }
        public double TaxableAmountinSys { get; set; }
        public double TaxableAmountFC { get; set; }
        public double TaxableAmount { get; set; }
        public object RoundingType { get; set; }
        public double Rate { get; set; }
        public string Criteria { get; set; }
        public string Category { get; set; }
        public string BaseType { get; set; }
        public double AppliedWTAmountSys { get; set; }
        public double AppliedWTAmountFC { get; set; }
        public double AppliedWTAmount { get; set; }
        public string GLAccount { get; set; }
        public int LineNum { get; set; }
        public int BaseDocEntry { get; set; }
        public object BaseDocLine { get; set; }
        public int BaseDocType { get; set; }
        public object BaseDocumentReference { get; set; }
        public string Status { get; set; }
        public int TargetAbsEntry { get; set; }
        public object TargetDocumentType { get; set; }
        public object CSTCodeIncoming { get; set; }
        public object CSTCodeOutgoing { get; set; }
    }
}