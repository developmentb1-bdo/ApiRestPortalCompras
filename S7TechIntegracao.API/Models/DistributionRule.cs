using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DistributionRule
    {
        public string FactorCode { get; set; }
        public string FactorDescription { get; set; }
        public double? TotalFactor { get; set; }
        public string Direct { get; set; }
        public int? InWhichDimension { get; set; }
        public string Active { get; set; }
        public string IsFixedAmount { get; set; }
        public List<DistributionRuleLine> DistributionRuleLines { get; set; }

        public DistributionRule()
        {

        }
    }
}