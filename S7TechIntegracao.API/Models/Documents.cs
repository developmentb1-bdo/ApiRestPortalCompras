using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class Documents
    {
        //[SwaggerExclude]
        public int DocEntry { get; set; }
        public int Code { get; set; }
        public int DocEntryPedido { get; set; }
        public int DocEntrySolicitacao { get; set; }
        public int? DocNum { get; set; }
        public string DocType { get; set; }
        public DateTime? DocDate { get; set; }
        public DateTime? DocDueDate { get; set; }
        public DateTime? TaxDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Address { get; set; }
        public string NumAtCard { get; set; }       
        public int? AttachmentEntry { get; set; } = null;
        public int? DocumentsOwner { get; set; }
        public int? ReqType { get; set; }
        public string Requester { get; set; }
        public string RequesterName { get; set; }
        public int? RequesterBranch { get; set; }
        public int? RequesterDepartment { get; set; }
        public string RequesterEmail { get; set; }
        public DateTime? RequriedDate { get; set; }
        public string DocCurrency { get; set; }
        public double? DocRate { get; set; }
        public string Reference1 { get; set; }
        public string Reference2 { get; set; }
        public string Comments { get; set; }
        public string OpeningRemarks { get; set; }
        public string JournalMemo { get; set; }
        public int? PaymentGroupCode { get; set; }
        [SwaggerExclude]      
        public TimeSpan? DocTime { get; set; }
        public int? SalesPersonCode { get; set; } = -1;
        public int? TransportationCode { get; set; }
        public string Confirmed { get; set; } = "tYES";
        public int? ContactPersonCode { get; set; }
        public string ShipToCode { get; set; }
        public string Indicator { get; set; }
        public string FederalTaxID { get; set; }

        //public double? DiscountPercent { get; set; }
        public string PaymentReference { get; set; }
        [SwaggerExclude]
        public DateTime? CreationDate { get; set; }
        [SwaggerExclude]
        public DateTime? UpdateDate { get; set; }
        public int? FinancialPeriod { get; set; }
        public int? TransNum { get; set; }
        public double? VatSum { get; set; }
        public double? VatSumSys { get; set; }
        public double? VatSumFc { get; set; }
        public double DocTotal { get; set; }
        public double? Saldo { get; set; }
        public double? PaidToDate { get; set; }
        public double? DocTotalFc { get; set; }
        public double? DocTotalSys { get; set; }
        [SwaggerExclude]
        public DateTime? CancelDate { get; set; }
        public int? Segment { get; set; }
        [SwaggerExclude]
        public string PickStatus { get; set; }
        [SwaggerExclude]
        public string Pick { get; set; }
        public string PaymentMethod { get; set; }
        public string Project { get; set; }
        public string AgentCode { get; set; }
        public string DocumentStatus { get; set; }
        public string PayToCode { get; set; }
        public int? BPL_IDAssignedToInvoice { get; set; }
        public string BPLName { get; set; }
        public int IdFilialIntBank { get; set; }
        public string InscMunicipalFilial { get; set; }
        public int? SequenceCode { get; set; }
        public int? SequenceSerial { get; set; }     
        public string SeriesString { get; set; }
        public string SubSeriesString { get; set; }
        public string SequenceModel { get; set; }
        public double? DownPayment { get; set; }
        public double? DownPaymentAmount { get; set; }
        public double? DownPaymentPercentage { get; set; }
        public string DownPaymentType { get; set; }
        public double? DownPaymentAmountSC { get; set; }
        public double? DownPaymentAmountFC { get; set; }
        public string Cancelled { get; set; }

        [SwaggerExclude]
        public TimeSpan? UpdateTime { get; set; }        
        public double? PaidToDateFC { get; set; }
        public double? PaidToDateSys { get; set; }
        public int? NumberOfInstallments { get; set; }
        public string HandWritten { get; set; }
        public string Posted { get; set; }
        public string DocObjectCode { get; set; }
        [MaxLength(2)]
        public string U_SKILL_TipTrib { get; set; }
        [MaxLength(2)]
        public string U_SKILL_IndISS { get; set; }
        public DateTime? U_SKILL_DTPRE { get; set; }
        [MaxLength(1)]
        public string U_S7T_Destinatario { get; set; }
        [MaxLength(15)]
        public string U_S7T_PnAlt { get; set; }
        [MaxLength(100)]
        public string U_S7T_PnAltNome { get; set; }
        [MaxLength(1)]
        public string U_S7T_TipoFatura { get; set; }
        [MaxLength(20)]
        public string U_S7T_PnAltCNPJ { get; set; }
        [MaxLength(30)]
        public string U_S7T_CodigoPI { get; set; }
        [MaxLength(1)]
        public string U_S7T_PnAltPapel { get; set; }
        public int? U_S7T_ValorBruto { get; set; }
        public int U_S7T_ValorDesconto { get; set; }   

        [MaxLength(11)]
        public string U_S7T_NumFatAnt { get; set; }
        [MaxLength(20)]
        public string U_S7T_CodeDebit { get; set; }
        public string U_S7T_LinkDocs { get; set; }
        public DateTime? U_S7T_DataSolicitacao { get; set; }
        public TimeSpan? U_S7T_HoraSolicitacao { get; set; }
        public TaxExtension TaxExtension { get; set; }
        public string StatusAprovacao { get; set; }
        public string CentroCusto { get; set; }
        public string U_SKILL_FormaPagto { get; set; }
        public string U_S7T_EnvEmailMala { get; set; }
        public string U_CodigoVerificador { get; set; }
        public string NumNfse { get; set; }
        public string U_LinlNFSe { get; set; }
        public string U_NrRPS { get; set; }     
        public string HourCreateTitle { get; set; }        
        public List<BoletoPdf> boletos { get; set; }
        public List<DocumentLines> DocumentLines { get; set; }
        public List<DocumentInstallments> DocumentInstallments { get; set; }
        public List<DownPaymentsToDraw> DownPaymentsToDraw { get; set; }  
        
        public List<PaymentInvoices> PaymentInvoices { get; set; }


        public Documents()
        {
            Posted = "tYES";
        }
    }

   
}