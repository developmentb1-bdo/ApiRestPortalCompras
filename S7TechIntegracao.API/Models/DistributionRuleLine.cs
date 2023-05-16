using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DistributionRuleLine
    {
        public string CenterCode { get; set; }
        public double? TotalInCenter { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public DistributionRuleLine()
        {

        }
    }
}