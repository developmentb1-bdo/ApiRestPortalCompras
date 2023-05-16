using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DownPaymentsToDraw
    {
        public int? DocEntry { get; set; }
        public DateTime? PostingDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public double? AmountToDraw { get; set; }
        public string DownPaymentType { get; set; }
        public double? AmountToDrawFC { get; set; }
        public double? AmountToDrawSC { get; set; }
        public int DocInternalID { get; set; }
        public int RowNum { get; set; }
        public int? DocNumber { get; set; }
        public double? Tax { get; set; }
        public double? TaxFC { get; set; }
        public double? TaxSC { get; set; }
        public double? GrossAmountToDraw { get; set; }
        public double? GrossAmountToDrawFC { get; set; }
        public double? GrossAmountToDrawSC { get; set; }
        public string IsGrossLine { get; set; }
        public List<DownPaymentsToDrawDetails> DownPaymentsToDrawDetails { get; set; }
    }
}