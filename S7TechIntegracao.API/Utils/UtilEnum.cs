using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace S7TechIntegracao.API.Utils {

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BusinessPartnersTaxIdOption
    {
        CNPJ = 1,
        CPF = 2,
        IDParceiroInternacional = 3
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BusinessPartnersTypeOption
    {
        Cliente = 1,
        Fornecedor = 2
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum DimensionOption
    {
        Dimensao1 = 1,
        Dimensao2 = 2,
        Dimensao3 = 3,
        Dimensao4 = 4,
        Dimensao5 = 5
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum EnderecoEnum {
        ENTREGA,
        [EnumMember(Value = "COBRANÇA")] COBRANCA,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoAddressType {
        bo_ShipTo,
        bo_BillTo,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoCardTypes {
        [EnumMember(Value = "cCustomer")] C,
        [EnumMember(Value = "cSupplier")] S,
        [EnumMember(Value = "cLid")] L,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoCardCompanyTypes {
        [EnumMember(Value = "cCompany")] C,
        [EnumMember(Value = "cPrivate")] I,
        [EnumMember(Value = "cGovernment")] G,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoPaymentTypeEnum {
        [EnumMember(Value = "Recebimento")] boptIncoming,
        [EnumMember(Value = "Pagamento")] boptOutgoing,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoPaymentMeansEnum {
        [EnumMember(Value = "Cheque")] bopmCheck,
        [EnumMember(Value = "Transferência bancária")] bopmBankTransfer,
        [EnumMember(Value = "Boleto")] bopmBillOfExchange,
    }
	[JsonConverter(typeof(StringEnumConverter))]
    public enum BoYesNoEnum {
        tNO,
        tYES
    }	

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoRcptInvTypes {
        it_AllTransactions,
        it_OpeningBalance,
        it_ClosingBalance,
        it_Invoice,
        it_CredItnote,
        it_TaxInvoice,
        it_Return,
        it_PurchaseInvoice,
        it_PurchaseCreditNote,
        it_PurchaseDeliveryNote,
        it_PurchaseReturn,
        it_Receipt,
        it_Deposit,
        it_JournalEntry,
        it_PaymentAdvice,
        it_ChequesForPayment,
        it_StockReconciliations,
        it_GeneralReceiptToStock,
        it_GeneralReleaseFromStock,
        it_TransferBetweenWarehouses,
        it_WorkInstructions,
        it_DeferredDeposit,
        it_CorrectionInvoice,
        it_APCorrectionInvoice,
        it_ARCorrectionInvoice,
        it_DownPayment,
        it_PurchaseDownPayment,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BoObjectTypes
    {
        [EnumMember(Value = "Pedido de Compra")] oPurchaseOrders = 22,
        [EnumMember(Value = "Solicitação de Compra")] oPurchaseRequest = 1470000113,
        [EnumMember(Value = "Oferta de Compra")] oPurchaseQuotations = 540000006,
    }

    public enum TipoLogin
    {
        Aprovador = 1,
        Criador = 2
    }
}