using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class BPAddresses
    {
        public string AddressName { get; set; }
        public string AddressType { get; set; }
        public string Street { get; set; }
        public string Block { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public int? County { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string FederalTaxID { get; set; }
        public string BuildingFloorRoom { get; set; }
        public string TypeOfAddress { get; set; }
        public string StreetNo { get; set; }
    }
}