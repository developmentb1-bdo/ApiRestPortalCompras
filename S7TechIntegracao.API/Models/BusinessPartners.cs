using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using S7TechIntegracao.API.Utils;

namespace S7TechIntegracao.API.Models
{
    public class BusinessPartners
    {
        public string CardCode { get; set; }
        public string CardName { get; set; }

        [MaxLength(20)]
        [Description("[S7T] CNPJ")]
        public string U_S7T_CNPJ { get; set; }
        [MaxLength(20)]
        [Description("[S7T] CPF")]
        public string U_S7T_CPF { get; set; }


        public string CardType { get; set; }

        public string CardForeignName { get; set; }

        public string AliasName { get; set; }

        public string Phone1 { get; set; }

        public string Phone2 { get; set; }

        public string EmailAddress { get; set; }
        public string Cellular { get; set; }
        public string TaxId0 { get; set; }
        public string TaxId1 { get; set; }
        public string TaxId3 { get; set; }
        public string TaxId4 { get; set; }
        public string TaxId5 { get; set; }
        public string Currency { get; set; }
        public string ShipToDefault { get; set; }
        public string BilltoDefault { get; set; }
        public string GroupCode { get; set; }
        public string GroupName { get; set; }
        public string U_EmailAC { get; set; }
        public string UpdateTime { get; set; }
        public string UpdateDate { get; set; }
        public string CreateDate { get; set; }
        public string CreateTime { get; set; }


        //**************** CONTATO *****************

        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string E_MailL { get; set; }
        public string Tel1 { get; set; }
        public string CurrentAccountBalance { get; set; }


        //***************** ENDEREÇO COBRANÇA *******************
        public List<BPAddresses> BPAddresses { get; set; }

        public string TaxId
        {
            get
            {
                if (string.IsNullOrEmpty(U_S7T_CNPJ))
                    U_S7T_CNPJ = string.Empty;

                if (string.IsNullOrEmpty(U_S7T_CPF))
                    U_S7T_CPF = string.Empty;

                if (string.IsNullOrEmpty(U_S7T_CNPJ))
                    return U_S7T_CPF;
                else
                    return U_S7T_CNPJ;
            }
        }
    }


    //public string Currency { get; set; }    
    //public double DocRate { get; set; } = 1.00;
}
