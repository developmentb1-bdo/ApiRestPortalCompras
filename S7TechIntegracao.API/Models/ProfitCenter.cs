using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class ProfitCenter
    {
        public string CenterCode { get; set; }
        public string CenterName { get; set; }
        public string GroupCode { get; set; }
        public int? InWhichDimension { get; set; }
        public string CostCenterType { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string Active { get; set; }
        public string U_S7T_AreaFuncional { get; set; }

        public ProfitCenter()
        {
            U_S7T_AreaFuncional = string.Empty;
        }
    }
}