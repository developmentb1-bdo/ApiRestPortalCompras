using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;

namespace S7TechIntegracao.API.Objetos
{
    public class DocumentsObj
    {
        

        private static readonly DocumentsObj _instancia = new DocumentsObj();

        public static DocumentsObj GetInstance()
        {
            return _instancia;
        }

        internal void AdicionarCentrosCustos(ref object modelo)
        {
            try
            {
                var modeloConvertido = JsonConvert.DeserializeObject<dynamic>(modelo.ToString());

                if (modeloConvertido.DocumentLines == null)
                    return;

                for (var i = 0; i < modeloConvertido.DocumentLines.Count; i++)
                {
                    var linha = modeloConvertido.DocumentLines[i];
                    var projetoCode = linha.ProjectCode;
                    var costingCode2 = linha.CostingCode2;

                    if (projetoCode != null)
                    {
                        Project projeto = ProjectsObj.GetInstance().Consultar(projetoCode.ToString());
                        modeloConvertido.DocumentLines[i].CostingCode3 = projeto.U_S7Segmento;
                    }

                    if (costingCode2 != null)
                    {
                        ProfitCenter profitCenter = ProfitCentersObj.GetInstance().Consultar(costingCode2.ToString());
                        modeloConvertido.DocumentLines[i].CostingCode = profitCenter.U_S7T_AreaFuncional;
                    }
                }

                modelo = JsonConvert.SerializeObject(modeloConvertido);
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DocumentsObj] [AdicionarCentrosCustos] {ex.Message}");

                throw ex;
            }
        }

        internal void VerificarCriacaoPagamento(Documents modelo)
        {
            try
            {
                foreach (var linha in modelo.DocumentLines)
                {
                    var utilizacao = linha.Usage;
                    var query = string.Format(S7Tech.GetConsultas("ConsultarParametrosBaixa"), utilizacao);
                    using (var hanaService = new HanaService())
                    {
                        var acctCode = Convert.ToString(hanaService.ExecuteScalar(query));

                        if (string.IsNullOrEmpty(acctCode))
                            continue;

                        var invoicesPagamento = new JArray();

                        foreach (var parcela in modelo.DocumentInstallments)
                        {
                            var invoicePagamento = new JObject();
                            invoicePagamento["DocEntry"] = modelo.DocEntry;
                            invoicePagamento["InstallmentId"] = parcela.InstallmentId;
                            invoicePagamento["SumApplied"] = parcela.Total;
                            invoicePagamento["InvoiceType"] = "it_Invoice";

                            invoicesPagamento.Add(invoicePagamento);
                        }

                        var pagamento = new JObject();
                        pagamento["CardCode"] = modelo.CardCode;
                        pagamento["DocDate"] = modelo.DocDate;
                        pagamento["TaxDate"] = modelo.TaxDate;
                        pagamento["DueDate"] = modelo.DocDueDate;
                        pagamento["TransferAccount"] = acctCode;
                        pagamento["TransferSum"] = modelo.DocTotal;
                        pagamento["TransferDate"] = modelo.DocDate;
                        pagamento["DocType"] = "rCustomer";
                        pagamento["DocTypte"] = "rCustomer";
                        pagamento["BPLID"] = modelo.BPL_IDAssignedToInvoice;
                        pagamento["PaymentInvoices"] = invoicesPagamento;

                        var modeloPagamento = JsonConvert.SerializeObject(pagamento);

                        IncomingPaymentsObj.GetInstance().Cadastrar(modeloPagamento);

                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[DocumentsObj] [VerificarCriacaoPagamento] {ex.Message}");

                throw ex;
            }
        }
    }
}