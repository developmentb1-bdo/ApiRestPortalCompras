using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DocumentSerialNumbers
    {
        public string ManufacturerSerialNumber { get; set; }
        public string InternalSerialNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ReceptionDate { get; set; }
        public DateTime? WarrantyStart { get; set; }
        public DateTime? WarrantyEnd { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public string BatchID { get; set; }
        public int? SystemSerialNumber { get; set; }
        public int? BaseLineNumber { get; set; }
        public double? Quantity { get; set; }
        public int? TrackingNote { get; set; }
        public int? TrackingNoteLine { get; set; }

    }
}