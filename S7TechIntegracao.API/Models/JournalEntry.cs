using S7TechIntegracao.API.Models;
using System;
using System.Collections.Generic;

namespace S7TechIntegracao.API.Controllers
{
    public class JournalEntry
    {

        public string ReferenceDate { get; set; }
        public string DueDate { get; set; }
        public string Memo { get; set; }
        public List<JournalEntryLines> JournalEntryLines { get; set; }

        //public string ReferenceDate { get; set; }
        //public string Memo { get; set; }
        //public string Reference { get; set; }
        //public string Reference2 { get; set; }      
        //public string TaxDate { get; set; }
        //public int JdtNum { get; set; }       
        //public int Series { get; set; }
        //public bool ShouldSerializeSeries()
        //{
        //    return Series != -1;
        //}
        //public string DueDate { get; set; }       
        //public int Number { get; set; }
        //public bool ShouldSerializeNumber()
        //{
        //    return Number != -1;
        //}
        //public int Original { get; set; }
        //public bool ShouldSerializeOriginal()
        //{
        //    return Original != -1;
        //}

        //public string BaseReference { get; set; }
        //public bool ShouldSerializeBaseReference()
        //{
        //    return !string.IsNullOrEmpty(BaseReference);
        //}

    }
    public class JournalVoucher
    {
        public JournalEntry JournalEntry { get; set; }

    }

    public class JournalVouchersService_Add
    {
        public JournalVoucher JournalVoucher { get; set; }
    }
}