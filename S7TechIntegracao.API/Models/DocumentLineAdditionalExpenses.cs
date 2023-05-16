using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DocumentLineAdditionalExpenses
    {
        public int? LineNumber { get; set; }
        public int? GroupCode { get; set; }
        public int? ExpenseCode { get; set; }
        public double? LineTotal { get; set; }
        public double? LineTotalFC { get; set; }
        public double? LineTotalSys { get; set; }
        public double? PaidToDate { get; set; }
        public double? PaidToDateFC { get; set; }
        public double? PaidToDateSys { get; set; }
        public string TaxLiable { get; set; }
        public string VatGroup { get; set; }
        public double? TaxPercent { get; set; }
        public double? TaxSum { get; set; }
        public double? TaxSumFC { get; set; }
        public double? TaxSumSys { get; set; }
        public double? DeductibleTaxSum { get; set; }
        public double? DeductibleTaxSumFC { get; set; }
        public double? DeductibleTaxSumSys { get; set; }
        public string AquisitionTax { get; set; }
        public string TaxCode { get; set; }
        public string TaxType { get; set; }
        public double? TaxPaid { get; set; }
        public double? TaxPaidFC { get; set; }
        public double? TaxPaidSys { get; set; }
        public double? TaxTotalSum { get; set; }
        public double? TaxTotalSumFC { get; set; }
        public double? TaxTotalSumSys { get; set; }
        public string WTLiable { get; set; }
        public int? BaseGroup { get; set; }
        public string DistributionRule { get; set; }
        public string Project { get; set; }
        public string DistributionRule2 { get; set; }
        public string DistributionRule3 { get; set; }
        public string DistributionRule4 { get; set; }
        public string DistributionRule5 { get; set; }
    }
}