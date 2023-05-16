using S7TechIntegracao.API.Models;
using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.App_Start
{
    public static class SapDIConfig
    {
        public static void GetConfigurationInitial()
        {
            var param = (NameValueCollection)ConfigurationManager.GetSection("ParametrosSAP");
            ParamsModel.GetInstance().HanaApi = param["HanaApi"];
            ParamsModel.GetInstance().CompanyDB = param["CompanyDB"];
            ParamsModel.GetInstance().UserName = param["UserName"];
            ParamsModel.GetInstance().Password = param["Password"];
            ParamsModel.GetInstance().Server = param["Server"];
            ParamsModel.GetInstance().DbUserName = param["DbUserName"];
            ParamsModel.GetInstance().DbPassword = param["DbPassword"];
            ParamsModel.GetInstance().LicenseServer = param["LicenseServer"];
            ParamsModel.GetInstance().DbServerType = Convert.ToInt32(param["DbServerType"]);
            ParamsModel.GetInstance().StringConnection = param["connString"];
            ParamsModel.GetInstance().Url = param["url"];

            var defSettings = (NameValueCollection)ConfigurationManager.GetSection("DefaultSettings");

            DefaultSettingsModel.GetInstance().Address = defSettings["Address"];
            DefaultSettingsModel.GetInstance().Host = defSettings["Host"];
            DefaultSettingsModel.GetInstance().EnableSsl = Convert.ToBoolean(defSettings["EnableSsl"]);
            DefaultSettingsModel.GetInstance().Port = Convert.ToInt32(defSettings["Port"]);
            DefaultSettingsModel.GetInstance().UserName = defSettings["UserName"];
            DefaultSettingsModel.GetInstance().Password = defSettings["Password"];
            DefaultSettingsModel.GetInstance().AltUserName = defSettings["AltUserName"];
            DefaultSettingsModel.GetInstance().AltPassword = defSettings["AltPassword"];
            DefaultSettingsModel.GetInstance().UpdateTaxDateNow = Convert.ToBoolean(defSettings["UpdateTaxDateNow"]);
            DefaultSettingsModel.GetInstance().PODefMailLayout = defSettings["PODefMailLayout"];
            DefaultSettingsModel.GetInstance().RQDefMailLayout = defSettings["RQDefMailLayout"];
            DefaultSettingsModel.GetInstance().PaginationLimit = Convert.ToInt32(defSettings["PaginationLimit"]);
            DefaultSettingsModel.GetInstance().ReceivPaginationLimit = Convert.ToInt32(defSettings["ReceivPaginationLimit"]);

            var paramInvent = (NameValueCollection)ConfigurationManager.GetSection("ParametrosInvent");

            InventDefaultSettingsModel.GetInstance().UrlInvent = paramInvent["UrlInvent"];
            InventDefaultSettingsModel.GetInstance().Authorization = paramInvent["Authorization"];
            InventDefaultSettingsModel.GetInstance().Extension = paramInvent["Extension"];
            InventDefaultSettingsModel.GetInstance().ContentType = paramInvent["ContentType"];

        }
    }
}