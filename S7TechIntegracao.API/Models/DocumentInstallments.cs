using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DocumentInstallments
    {
        public int DocEntry { get; set; }
        public DateTime? DueDate { get; set; }
        public double? Percentage { get; set; }
        public double? Total { get; set; }
        public DateTime? LastDunningDate { get; set; }
        public int? DunningLevel { get; set; }
        public double? TotalFC { get; set; }
        public int InstallmentId { get; set; }
        public string PaymentOrdered { get; set; }
        public string U_IB_GerarBoleto { get; set; }
        public string OpeningRemarks { get; set; }
        public string Baixado { get; set; }
        

    }
}