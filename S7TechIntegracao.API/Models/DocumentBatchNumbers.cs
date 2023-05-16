using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DocumentBatchNumbers
    {
        public string BatchNumber { get; set; }
        public string ManufacturerSerialNumber { get; set; }
        public string InternalSerialNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ManufacturingDate { get; set; }
        public DateTime? AddmisionDate { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public double? Quantity { get; set; }
        public int? BaseLineNumber { get; set; }
        public int? TrackingNote { get; set; }
        public int? TrackingNoteLine { get; set; }

    }
}