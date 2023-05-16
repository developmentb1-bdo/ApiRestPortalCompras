using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DocumentLinesBinAllocations
    {
        public int? BinAbsEntry { get; set; }
        public double? Quantity { get; set; }
        public string AllowNegativeQuantity { get; set; }
        public int? SerialAndBatchNumbersBaseLine { get; set; }
        public int? BaseLineNumber { get; set; }
    }
}