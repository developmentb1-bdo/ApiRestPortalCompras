using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class JournalEntryLines
    {


        public string AccountCode { get; set; }
        public string LineMemo { get; set; }
        public int Debit { get; set; }
        public int Credit { get; set; }
        public string ProjectCode { get; set; }
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }
        public int BPLID { get; set; }


        //public int Line_ID { get; set; }

        //public bool ShouldSerializeLineId()
        //{
        //    return Line_ID != -1;
        //}

        //public string AccountCode { get; set; }
        //public double Debit { get; set; }
        //public double Credit { get; set; }
        //public string DueDate { get; set; }
        //public string ShortName { get; set; }
        //public string ContraAccount { get; set; }
        //public string LineMemo { get; set; }
        //public string ReferenceDate1 { get; set; }
        //public bool ShouldSerializeReferenceDate1()
        //{
        //    return !string.IsNullOrEmpty(ReferenceDate1);
        //}
        //public string ReferenceDate2 { get; set; }

        //public bool ShouldSerializeReferenceDate2()
        //{
        //    return !string.IsNullOrEmpty(ReferenceDate2);
        //}
        //public string Reference1 { get; set; }

        //public bool ShouldSerializeReference1()
        //{
        //    return !string.IsNullOrEmpty(Reference1);
        //}
        //public string Reference2 { get; set; }
        //public bool ShouldSerializeReference2()
        //{
        //    return !string.IsNullOrEmpty(Reference2);
        //}
        //public string ProjectCode { get; set; }
        //public string CostingCode { get; set; }
        //public string TaxDate { get; set; }
        //public bool ShouldSerializeTaxDate()
        //{
        //    return !string.IsNullOrEmpty(TaxDate);
        //}
        //public string AdditionalReference { get; set; }

        //public bool ShouldSerializeAdditionalReference()
        //{
        //    return !string.IsNullOrEmpty(AdditionalReference);
        //}
        //public string CostingCode2 { get; set; }
        //public string CostingCode3 { get; set; }
        //public bool ShouldSerializeCostingCode3()
        //{
        //    return !string.IsNullOrEmpty(CostingCode3);
        //}
        //public string CostingCode4 { get; set; }
        //public bool ShouldSerializeCostingCode4()
        //{
        //    return !string.IsNullOrEmpty(CostingCode4);
        //}
        //public string CostingCode5 { get; set; }

        //public bool ShouldSerializeCostingCode5()
        //{
        //    return !string.IsNullOrEmpty(CostingCode5);
        //}
        //public int BPLID { get; set; }
        //public string BPLName { get; set; }

        //public bool ShouldSerializeBPLName()
        //{
        //    return !string.IsNullOrEmpty(BPLName);
        //}
        //public string VATRegNum { get; set; }

        //public bool ShouldSerializeVATRegNum()
        //{
        //    return !string.IsNullOrEmpty(VATRegNum);
        //}

    }
}