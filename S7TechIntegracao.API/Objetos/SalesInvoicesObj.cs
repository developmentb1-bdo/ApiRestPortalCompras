using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace S7TechIntegracao.API.Objetos
{
    public class SalesInvoicesObj
    {
        private static readonly SalesInvoicesObj _instancia = new SalesInvoicesObj();

        public static SalesInvoicesObj GetInstance()
        {
            return _instancia;
        }
        public Documents Cadastrar(object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                DocumentsObj.GetInstance().AdicionarCentrosCustos(ref model);

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("Invoices", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [Cadastrar] {ex.Message}");

                throw ex;
            }
        }
        public Documents Atualizar(int docEntry, object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Invoices({docEntry})", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                //var response = client.Execute<Documents>(request);

                var response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var document = Consultar(docEntry);

                return document;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [Atualizar] {ex.Message}");

                throw ex;
            }
        }
        public List<Documents> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var ret = new
                {
                    value = new List<BusinessPartners>()
                };

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("Invoices", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [ConsultarTodos] {ex.Message}");

                throw ex;
            }
        }
        public Documents Consultar(int docEntry)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Invoices({docEntry})", Method.GET);
                var response = client.Execute<Documents>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [Consultar] {ex.Message}");

                throw ex;
            }
        }
        public Documents ConsultarPorDocNum(int docNum)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"Invoices?$filter=DocNum eq {docNum}", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<Documents>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data.value.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [ConsultarPorDocNum] {ex.Message}");

                throw ex;
            }
        }    
        public List<Documents> ConsultarPorNumAtCard(string numAtCard)
        {
            try
            {
                var ret = new List<Documents>();
                var retDocEntry = new List<int>();
                var query = string.Format(S7Tech.GetConsultas("ConsultarNumAtCardSalesInvoice"), numAtCard);
                using (var hanaService = new HanaService())
                {
                    retDocEntry = hanaService.GetHanaConnection().Query<int>(query).ToList();
                }

                foreach (var docEntry in retDocEntry)
                {
                    ret.Add(Consultar(docEntry));
                }

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [ConsultarPorNumAtCard] {ex.Message}");

                throw ex;
            }
        }
        public List<Documents> ConsultarTitulosAbertos(string date, string hour, string codigoCliente, int docEntry, int pagina)
        {
            try
            {
                
                var ret = new List<Documents>();               
                var retDocEntry1 = new List<Documents>();
                var retBoleto = new List<BoletoLista>();
                var retPagamentos = new List<DocumentInstallments>();

                var dataDocumento = $@"AND ((TO_VARCHAR (TO_DATE(A.""CreateDate""), 'YYYYMMDD')|| ':'|| A.""DocTime"" > '{date}:{hour}') OR (TO_VARCHAR (TO_DATE(A.""UpdateDate""), 'YYYYMMDD')|| ':'|| A.""DocTime"" > '{date}:{hour}'))";
                var idDocumento =  $@"AND A.""DocEntry"" =  '{docEntry}' ";
                var cardCode = $@"AND A.""CardCode"" = '{codigoCliente}'";
                var parametroSap = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var companyDb = parametroSap["CompanyDB"];
                var paramInvent = (NameValueCollection)ConfigurationManager.GetSection("ParametrosInvent");
                var urlInvent = paramInvent["UrlInvent"];
                var token = paramInvent["Authorization"];
                var extension = paramInvent["Extension"];
                var contentType = paramInvent["ContentType"];

                var limit = DefaultSettingsModel.GetInstance().ReceivPaginationLimit;

                var offSet = (pagina - 1) * limit;

                if (string.IsNullOrEmpty(codigoCliente))
                {
                    cardCode = "";
                }
                if (string.IsNullOrEmpty(date) && string.IsNullOrEmpty(date))
                {
                    dataDocumento = "";
                }
                if (docEntry == 0)
                {
                    idDocumento = "";
                }
                var query = string.Format(S7Tech.GetConsultas("ConsultarOpenTitlesSalesInvoice"), dataDocumento, cardCode, idDocumento, companyDb,limit, offSet);                
                using (var hanaService = new HanaService())
                {
                    retDocEntry1 = hanaService.GetHanaConnection().Query<Documents>(query).ToList();                    
                }
                if (retDocEntry1.Count > 0)
                {                    
                    foreach (var item in retDocEntry1)
                    {
                        var documento = Consultar(item.DocEntry);                       

                        documento.FederalTaxID = item.FederalTaxID;
                        documento.Cancelled = item.Cancelled;
                        documento.IdFilialIntBank = item.IdFilialIntBank;
                        documento.InscMunicipalFilial = item.InscMunicipalFilial;

                        query = string.Format(S7Tech.GetConsultas("ConsultarInfoNFSeSKILL"), documento.DocEntry);
                        using (var hanaService = new HanaService())
                        {
                            retDocEntry1 = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                        }
                        foreach (var item1 in retDocEntry1)
                        {
                            documento.U_CodigoVerificador = item1.U_CodigoVerificador;
                            documento.NumNfse = item1.NumNfse;
                            documento.U_LinlNFSe = item1.U_LinlNFSe;
                            documento.U_NrRPS = item.U_NrRPS; 
                        }                   
                       
                        List<DocumentInstallments> documentInstallments = new List<DocumentInstallments>();

                        foreach (var item4 in documento.DocumentInstallments)
                        {   
                            
                            DocumentInstallments pag = new DocumentInstallments();                       
                            
                            var query1 = string.Format(S7Tech.GetConsultas("ConsultarTitlesPaidOutSalesInvoice"), documento.DocEntry, item4.InstallmentId);

                            using (var hanaService = new HanaService())
                            {
                                retPagamentos = hanaService.GetHanaConnection().Query<DocumentInstallments>(query1).ToList();

                                foreach (var item5 in retPagamentos)
                                {
                                    pag.DocEntry = documento.DocEntry;
                                    pag.Baixado = item5.Baixado;
                                    pag.InstallmentId = item4.InstallmentId;
                                    pag.Percentage = item4.Percentage;
                                    pag.OpeningRemarks = item.OpeningRemarks;
                                    pag.DueDate = item4.DueDate;
                                    pag.Total = item4.Total;
                                    pag.TotalFC = item4.TotalFC;
                                    pag.U_IB_GerarBoleto = item4.U_IB_GerarBoleto;
                                    pag.LastDunningDate = item4.LastDunningDate;
                                    pag.PaymentOrdered = item4.PaymentOrdered;
                                    pag.DunningLevel = item4.DunningLevel;
                                }
                             
                            }   
                           documentInstallments.Add(pag);                            
                        }
                        documento.DocumentInstallments.RemoveAll(i => i.DocEntry == documento.DocEntry);
                        
                        documento.DocumentInstallments = documentInstallments;

                        ret.Add(documento);

                        query = string.Format(S7Tech.GetConsultas("ConsultarIDBoletoInvent"), documento.DocEntry);
                        using (var hanaService = new HanaService())
                        {
                            retBoleto = hanaService.GetHanaConnection().Query<BoletoLista>(query).ToList();
                        }
                        List<BoletoPdf> boletos = new List<BoletoPdf>();
                        foreach (var item2 in retBoleto)
                        {
                            try
                            {

                                var client = new RestClient($@"{urlInvent} + {item2.codigo} + {extension}");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.GET);
                                request.AddHeader("Authorization", token);
                                request.AddHeader("Content-Type", contentType);
                                IRestResponse response = client.Execute(request);
                                Log4Net.Log.Error($"Status do retorno" + response.StatusCode + response.StatusDescription);
                                var arquivo64 = Convert.ToBase64String(response.RawBytes);

                                BoletoPdf insboleto = new BoletoPdf();
                                insboleto.codigo = item2.codigo;
                                insboleto.parcela = item2.parcela;
                                insboleto.boleto = arquivo64;

                                boletos.Add(insboleto);                             
                                
                            }
                            catch (Exception)
                            {
                                throw;
                            }

                        }
                        documento.boletos = boletos;

                        ret.Add(documento);
                    }
                }
                

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [ConsultarTitulosAbertos] {ex.Message}");

                throw ex;
            }
        }
        public List<Documents> ConsultarTitulosBaixados(string date, string hour, string codigoCliente, int docEntry, int pagina)
        {
            try
            {
                var ret = new List<Documents>();                
                var retDocEntry1 = new List<Documents>();
                var retPagamentos = new List<PaymentInvoices>();

                var dataDocumento = $@"AND (TO_VARCHAR (TO_DATE(E.""DocDate""), 'YYYYMMDD')|| ':'|| E.""DocTime"" > '{date}:{hour}' OR TO_VARCHAR(TO_DATE(E.""UpdateDate""), 'YYYYMMDD') >='{date}') ";
                var idDocumento = $@"AND A.""DocEntry"" =  '{docEntry}'";
                var cardCode = $@"AND A.""CardCode"" = '{codigoCliente}'";
                var parametroSap = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var companyDb = parametroSap["CompanyDB"];

                var limit = DefaultSettingsModel.GetInstance().ReceivPaginationLimit;

                var offSet = (pagina - 1) * limit;

                if (string.IsNullOrEmpty(codigoCliente))
                {
                    cardCode = "";
                }
                if (string.IsNullOrEmpty(date) && string.IsNullOrEmpty(date))
                {
                    dataDocumento = "";
                }
                if (docEntry == 0)
                {
                    idDocumento = "";
                }                
                var query = string.Format(S7Tech.GetConsultas("ConsultarDownloadedTitlesSalesInvoice"), dataDocumento, cardCode, idDocumento, companyDb, limit, offSet);

                using (var hanaService = new HanaService())
                {
                    retDocEntry1 = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                }  
                
                if (retDocEntry1.Count > 0)
                {
                    foreach (var item in retDocEntry1)
                    {
                        var documento = Consultar(item.DocEntry);
                        documento.Cancelled = item.Cancelled;
                        documento.Saldo = Math.Round((double)(documento.DocTotal - documento.PaidToDate), 2);
                        documento.IdFilialIntBank = item.IdFilialIntBank;
                        documento.InscMunicipalFilial = item.InscMunicipalFilial;
                        //documento.FederalTaxID = item.FederalTaxID;

                        query = string.Format(S7Tech.GetConsultas("ConsultarInfoNFSeSKILL"), documento.DocEntry);
                        using (var hanaService = new HanaService())
                        {
                            retDocEntry1 = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                        }

                        foreach (var item1 in retDocEntry1)
                        {
                            documento.U_CodigoVerificador = item1.U_CodigoVerificador;
                            documento.NumNfse = item1.NumNfse;
                            documento.U_LinlNFSe = item1.U_LinlNFSe;
                            documento.U_NrRPS = item.U_NrRPS;
                        }
                        query = string.Format(S7Tech.GetConsultas("ConsultarPaymentsTitlesSalesInvoice"), documento.DocEntry,companyDb);
                        using (var hanaService = new HanaService())
                        {
                            retPagamentos = hanaService.GetHanaConnection().Query<PaymentInvoices>(query).ToList();
                        }
                        List<PaymentInvoices> pagamentos = new List<PaymentInvoices>();
                        
                        foreach (var item2 in retPagamentos)
                        {
                            PaymentInvoices pag = new PaymentInvoices();
                            pag.InstallmentId = item2.InstallmentId;
                            pag.SumApplied =item2.SumApplied;                           
                            pag.DataPagamento = item2.DataPagamento;
                            pag.Canceled = item2.Canceled;
                            pag.Saldo = (Convert.ToDouble(item2.InsTotal)) - (Convert.ToDouble(item2.PaidToDate));
                            pag.ContaDoRazao = item2.ContaDoRazao;
                            pag.NomeDaConta = item2.NomeDaConta;
                            pag.BaixaRenegociacao = item2.BaixaRenegociacao;
                            pagamentos.Add(pag);
                        }                        
                        documento.PaymentInvoices = pagamentos;
                        ret.Add(documento);
                    }
                   
                }
                else
                {
                    Console.WriteLine("Nota Fiscal Cancelada ou inexistente!");
                }
                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[SalesInvoicesObj] [ConsultarTitulosFechados] {ex.Message}");

                throw ex;
            }            
        }
        public List<Documents> ConsultarTitulosCanceladosOuFechados(string date, string hour, string codigoCliente, int docEntry, int pagina)
        {
            try
            {
                var ret = new List<Documents>();
                var retDocEntry1 = new List<Documents>();              
                              
                var dataDocumento = $@"AND (TO_VARCHAR (TO_DATE(A.""CreateDate""), 'YYYYMMDD')|| ':'|| A.""DocTime"" > '{date}:{hour}' OR TO_VARCHAR(TO_DATE(A.""UpdateDate""), 'YYYYMMDD') >='{date}') ";
                var idDocumento = $@"AND A.""DocEntry"" =  '{docEntry}' ";
                var cardCode = $@"AND A.""CardCode"" = '{codigoCliente}'";
                var parametroSap = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var companyDb = parametroSap["CompanyDB"];

                var limit = DefaultSettingsModel.GetInstance().ReceivPaginationLimit;              
                var offSet = (pagina - 1) * limit;
                if (string.IsNullOrEmpty(codigoCliente))
                {
                    cardCode = "";
                }
                if (string.IsNullOrEmpty(date) && string.IsNullOrEmpty(date))
                {
                    dataDocumento = "";
                }
                if (docEntry == 0)
                {
                    idDocumento = "";
                }
                
                var query = string.Format(S7Tech.GetConsultas("ConsultarTitlesCanceledOrClosedSalesInvoice"), dataDocumento, cardCode, idDocumento, companyDb, limit, offSet);                
                using (var hanaService = new HanaService())
                {
                    retDocEntry1 = hanaService.GetHanaConnection().Query<Documents>(query).ToList();
                }
                if (retDocEntry1.Count > 0)
                {
                    foreach (var item in retDocEntry1)
                    {
                        var documento = Consultar(item.DocEntry);

                        documento.FederalTaxID = item.FederalTaxID;
                        documento.Cancelled = item.Cancelled;
                        documento.IdFilialIntBank = item.IdFilialIntBank;                       

                        ret.Add(documento);
                    }                    
                }
                return ret;
            }
            catch (Exception ex)
            {

                Log4Net.Log.Error($"[SalesInvoicesObj] [ConsultarTitulosCanceladosOuFechados] {ex.Message}");

                throw ex;
            }
        }
    }
}