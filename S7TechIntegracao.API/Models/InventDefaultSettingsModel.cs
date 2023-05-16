using System;

namespace S7TechIntegracao.API.Models
{
    public class InventDefaultSettingsModel
    {
        private static readonly InventDefaultSettingsModel _instancia = new InventDefaultSettingsModel();

        public static InventDefaultSettingsModel GetInstance()
        {
            try
            {
                return _instancia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string UrlInvent { get; set; }
        public string Authorization { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }

    }
}