using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class BusinessPlace
    {
        public int BPLID { get; set; }
        public string BPLName { get; set; }
        public string FederalTaxID { get; set; }

        public BusinessPlace()
        {

        }
    }
}