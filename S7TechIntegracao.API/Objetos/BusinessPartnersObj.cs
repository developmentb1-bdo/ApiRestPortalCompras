using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using S7TechIntegracao.API.Models;
using S7TechIntegracao.API.Utils;

namespace S7TechIntegracao.API.Objetos
{
    public class BusinessPartnersObj
    {


        private static readonly BusinessPartnersObj _instancia = new BusinessPartnersObj();

        public static BusinessPartnersObj GetInstance()
        {
            return _instancia;
        }

        public BusinessPartners Cadastrar(object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("BusinessPartners", Method.POST);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                var response = client.Execute<BusinessPartners>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [Cadastrar] {ex.Message}");
                throw ex;
            }
        }

        public BusinessPartners Atualizar(string cardCode, object model)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                var client = Conexao.GetInstance().Client;
                client.Timeout = -1;
                var request = new RestRequest($"BusinessPartners('{cardCode}')", Method.PATCH);
                request.AddParameter("application/json", model, ParameterType.RequestBody);
                //request.AddJsonBody(model);
                var response = client.Execute(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var businessPartiner = Consultar(cardCode);

                return businessPartiner;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [Atualizar] {ex.Message}");
                throw ex;
            }
        }

        public List<BusinessPartners> ConsultarTodos()
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                //var currency = SBOBobServiceObj.GetInstance().GetSystemCurrency();

                var ret = new
                {
                    value = new List<BusinessPartners>()
                };

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest("BusinessPartners", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<BusinessPartners>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                //foreach (var item in response.Data.value.Where(bp => bp.Currency != currency))
                //{
                //    item.DocRate = (double)SBOBobServiceObj.GetInstance().GetCurrencyRate(item.Currency, DateTime.Now);
                //}

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [ConsultarTodos] {ex.Message}");
                throw ex;
            }
        }

        public BusinessPartners Consultar(string cardCode)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                //var currency = SBOBobServiceObj.GetInstance().GetSystemCurrency();

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"BusinessPartners('{cardCode}')", Method.GET);
                var response = client.Execute<BusinessPartners>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                //if (response.Data.Currency != currency)
                //{
                //    response.Data.DocRate = (double)SBOBobServiceObj.GetInstance().GetCurrencyRate(response.Data.Currency, DateTime.Now);
                //}

                return response.Data;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [Consultar] {ex.Message}");
                throw ex;
            }
        }

        public List<BusinessPartners> ConsultarDateTime(string date, int skip, string operacao)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];
                var client = Conexao.GetInstance().Client;

                var limit = DefaultSettingsModel.GetInstance().ReceivPaginationLimit;

                var offSet = (skip - 1) * limit;

                var dataFilter = "";
                if (operacao == "C")
                {
                    dataFilter = $@"CreateDate ge '{date}'";
                }
                else if (operacao == "U")
                {
                    dataFilter = $@"UpdateDate ge '{date}'";
                }
                else
                {
                    throw new Exception("Parametro de Operação incorreto, informar {C} para criados e {U} para atualizado");
                }
                var request = new RestRequest($"BusinessPartners?$skip={offSet}&$filter={dataFilter} and CardType eq 'C' and CurrentAccountBalance gt 0", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<BusinessPartners>>>(request);
                var ret = JsonConvert.DeserializeObject<RetornoListaGenerica<List<BusinessPartners>>>(response.Content);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                var groupName = new List<BusinessPartners>();

                foreach (var item in response.Data.value)
                {
                    var query = string.Format(S7Tech.GetConsultas("ConsultarGroupName"), item.GroupCode);
                    using (var hanaService = new HanaService())
                    {
                        groupName = hanaService.GetHanaConnection().Query<BusinessPartners>(query).ToList();

                        foreach (var item1 in groupName)
                        {
                            item.GroupName = item1.GroupName;
                        }
                    }

                }

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [Consultar] {ex.Message}");
                throw ex;
            }
        }

        public List<BusinessPartners> ConsultarCardCode(string cardCode)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                //var currency = SBOBobServiceObj.GetInstance().GetSystemCurrency();

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"BusinessPartners?$filter=contains(CardCode, '{cardCode}')", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<BusinessPartners>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                //foreach (var item in response.Data.value.Where(bp => bp.Currency != currency))
                //{
                //    item.DocRate = (double)SBOBobServiceObj.GetInstance().GetCurrencyRate(item.Currency, DateTime.Now);
                //}

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [ConsultarCardCode] {ex.Message}");
                throw ex;
            }
        }

        public List<BusinessPartners> ConsultarCardName(string cardName)
        {
            try
            {
                var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
                var hanaApi = param["HanaApi"];

                //var currency = SBOBobServiceObj.GetInstance().GetSystemCurrency();

                var client = Conexao.GetInstance().Client;
                var request = new RestRequest($"BusinessPartners?$filter=contains(CardName, '{cardName}')", Method.GET);
                var response = client.Execute<RetornoListaGenerica<List<BusinessPartners>>>(request);

                if (!response.IsSuccessful)
                    throw new Exception(!string.IsNullOrEmpty(response.ErrorMessage) ? response.ErrorMessage : response.Content);

                //foreach (var item in response.Data.value.Where(bp => bp.Currency != currency))
                //{
                //    item.DocRate = (double)SBOBobServiceObj.GetInstance().GetCurrencyRate(item.Currency, DateTime.Now);
                //}

                return response.Data.value;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [ConsultarCardName] {ex.Message}");
                throw ex;
            }
        }
        public BusinessPartners ConsultarByTaxId(string taxId, BusinessPartnersTaxIdOption option, BusinessPartnersTypeOption tipo)
        {
            try
            {
                var where = string.Empty;
                var cardType = "CardType eq 'cCustomer'";

                if (tipo == BusinessPartnersTypeOption.Fornecedor)
                    cardType = "CardType eq 'cSupplier'";
                using (var hanaService = new HanaService())
                {
                    cardType = "C";

                    if (tipo == BusinessPartnersTypeOption.Fornecedor)
                        cardType = "S";

                    var query = string.Format(S7Tech.GetConsultas("ConsultarParceiroCPFCNPJ"), taxId, cardType);
                    var cardCode = hanaService.GetHanaConnection().Query<string>(query).FirstOrDefault();

                    if (!string.IsNullOrEmpty(cardCode))
                        return Consultar(cardCode);

                    dynamic error = new
                    {
                        code = -1,
                        errorMessage = $"Parceiro com TaxId {taxId} não encontrado"
                    };

                    var js = JsonConvert.SerializeObject(error);
                    throw new Exception(js);
                }
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [ConsultarByTaxId] {ex.Message}");
                throw ex;
            }
        }
        public List<BusinessPartners> ConsultarClientes()
        {
            try
            {
                return ConsultarClientesFornecedores("cCustomer");
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [ConsultarClientes] {ex.Message}");
                throw ex;
            }
        }

        public List<BusinessPartners> ConsultarFornecedores()
        {
            try
            {
                return ConsultarClientesFornecedores("cSupplier");
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [ConsultarFornecedores] {ex.Message}");
                throw ex;
            }
        }

        private List<BusinessPartners> ConsultarClientesFornecedores(string v)
        {
            try
            {
                var ret = new List<BusinessPartners>();
                var query = S7Tech.GetConsultas("ConsultarFornecedoresPortalCompras");

                //var currency = SBOBobServiceObj.GetInstance().GetSystemCurrency();

                using (var hanaService = new HanaService())
                {
                    ret = hanaService.GetHanaConnection().Query<BusinessPartners>(query).ToList();
                }

                //foreach (var item in ret.Where(bp => bp.Currency != currency))
                //{
                //    item.DocRate = (double)SBOBobServiceObj.GetInstance().GetCurrencyRate(item.Currency, DateTime.Now);
                //}

                return ret;
            }
            catch (Exception ex)
            {
                Log4Net.Log.Error($"[BusinessPartnersObj] [ConsultarFornecedoresPortal] {ex.Message}");
                throw ex;
            }
        }
    }
}