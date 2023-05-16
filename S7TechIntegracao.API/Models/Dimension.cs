using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class Dimension
    {
        public int DimensionCode { get; set; }
        public string DimensionName { get; set; }
        public string DimensionDescription { get; set; }
    }
}