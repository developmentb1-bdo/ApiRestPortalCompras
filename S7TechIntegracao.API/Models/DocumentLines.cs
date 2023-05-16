using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class DocumentLines
    {
        public int DocEntry { get; set; }
        public int? LineNum { get; set; }
        public int? BaseEntry { get; set; }
        public int? BaseLine { get; set; }
        public int? BaseType { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public double? Quantity { get; set; }
        public DateTime? ShipDate { get; set; }
        public double? Price { get; set; }
        public double? PriceAfterVAT { get; set; }
        public string Currency { get; set; }
        public double? Rate { get; set; }
        public double? DiscountPercent { get; set; }
        public string VendorNum { get; set; }
        public string SerialNum { get; set; }
        public string WarehouseCode { get; set; }
        public int? SalesPersonCode { get; set; } = -1;
        public double? CommisionPercent { get; set; } = 0;
        public string AccountCode { get; set; }
        public string CostingCode { get; set; }
        public string CostingCode2 { get; set; }
        public string CostingCode3 { get; set; }
        public string CostingCode4 { get; set; }
        public string CostingCode5 { get; set; }
        public string ProjectCode { get; set; }
        public string BarCode { get; set; }
        public string VatGroup { get; set; }
        public double? Height1 { get; set; }
        public int? Hight1Unit { get; set; }
        public double? Height2 { get; set; }
        public int? Height2Unit { get; set; }
        public double? Lengh1 { get; set; }
        public int? Lengh1Unit { get; set; }
        public double? Lengh2 { get; set; }
        public int? Lengh2Unit { get; set; }
        public double? Weight1 { get; set; }
        public int? Weight1Unit { get; set; }
        public double? Weight2 { get; set; }
        public int? Weight2Unit { get; set; }
        public double? Volume { get; set; }
        public int? VolumeUnit { get; set; }
        public double? Width1 { get; set; }
        public int? Width1Unit { get; set; }
        public double? Width2 { get; set; }
        public int? Width2Unit { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public double? LineTotal { get; set; }
        public double? RowTotalFC { get; set; }
        public double? TaxPercentagePerRow { get; set; }
        public double? TaxTotal { get; set; }
        public string CFOPCode { get; set; }
        public string CSTCode { get; set; }
        public string Usage { get; set; }
        public string TaxOnly { get; set; }
        public double? UnitPrice { get; set; }
        public int? LocationCode { get; set; }
        public double? OpenAmount { get; set; }
        public double? OpenAmountFC { get; set; }
        public double? OpenAmountSC { get; set; }
        public int? AgreementNo { get; set; }
        public int? AgreementRowNumber { get; set; }
        public int? UoMEntry { get; set; }
        public string UoMCode { get; set; }
        public int? NCMCode { get; set; }
        public string ShipToCode { get; set; }
        public string ShipToDescription { get; set; }
        public string ShipFromCode { get; set; }
        public string ShipFromDescription { get; set; }
        public string FreeText { get; set; }
        public string U_S7T_InventryNo { get; set; }
        public string U_S7T_FinalidadeMoviment { get; set; }
        public string U_S7T_TipoFaturamento { get; set; }
        public string U_S7T_Details { get; set; }
        public string Text { get; set; }
        public string U_S7T_NAT_BC_CRED { get; set; }
        public string U_RegPC {get;set;}
        public string U_IND_OPER { get; set; }        
        public string U_IND_ORIG_CRED { get; set; }

        public string U_SKILL_DOCUMENTO { get; set; }
        public int U_SKILL_DOCENTRY { get; set; }
        public string U_SKILL_ITEMCODE { get; set; }
        public int U_SKILL_LINHA { get; set; }

        public List<DocumentLineAdditionalExpenses> DocumentLineAdditionalExpenses { get; set; }
        public List<WithholdingTaxLines> WithholdingTaxLines { get; set; }
        public List<DocumentSerialNumbers> SerialNumbers { get; set; }
        public List<DocumentBatchNumbers> BatchNumbers { get; set; }
        public List<DocumentLinesBinAllocations> DocumentLinesBinAllocations { get; set; }
        
    }
}