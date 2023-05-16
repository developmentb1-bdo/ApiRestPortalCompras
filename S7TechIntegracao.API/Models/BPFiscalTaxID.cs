using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class BPFiscalTaxID
    {
        public string Address { get; set; }
        public int CNAECode { get; set; }
        public string TaxId0 { get; set; }
        public string TaxId1 { get; set; }
        public string TaxId2 { get; set; }
        public string TaxId3 { get; set; }
        public string TaxId4 { get; set; }
        public string TaxId5 { get; set; }
        public string TaxId6 { get; set; }
        public string TaxId7 { get; set; }
        public string TaxId8 { get; set; }
        public string TaxId9 { get; set; }
        public string TaxId10 { get; set; }
        public string TaxId11 { get; set; }
        [Required]
        public string BPCode { get; set; }
        public string AddrType { get; set; } = "bo_ShipTo";
        public string TaxId12 { get; set; }
        public string TaxId13 { get; set; }
    }
}