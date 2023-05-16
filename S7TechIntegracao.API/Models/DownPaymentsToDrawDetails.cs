using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DownPaymentsToDrawDetails
    {
        public int DocInternalID { get; set; }
        public int? RowNum { get; set; }
        public int SeqNum { get; set; }
        public int? DocEntry { get; set; }
        public string VatGroupCode { get; set; }
        public double? VatPercent { get; set; }
        public double? AmountToDraw { get; set; }
        public double? AmountToDrawFC { get; set; }
        public double? AmountToDrawSC { get; set; }
        public double? Tax { get; set; }
        public double? TaxFC { get; set; }
        public double? TaxSC { get; set; }
        public string IsGrossLine { get; set; }
        public double? GrossAmountToDraw { get; set; }
        public double? GrossAmountToDrawFC { get; set; }
        public double? GrossAmountToDrawSC { get; set; }
        public string LineType { get; set; }
        public string TaxAdjust { get; set; }
    }
}