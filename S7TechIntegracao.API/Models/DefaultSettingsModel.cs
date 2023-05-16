using System;

namespace S7TechIntegracao.API.Models
{
    public class DefaultSettingsModel
    {
        private static readonly DefaultSettingsModel _instancia = new DefaultSettingsModel();

        public static DefaultSettingsModel GetInstance()
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

        public string Address { get; set; }
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AltUserName { get; set; }
        public string AltPassword { get; set; }
        public bool UpdateTaxDateNow { get; set; }
        public string PODefMailLayout { get; set; }
        public string RQDefMailLayout { get; set; }
        public int PaginationLimit { get; set; }
        public int ReceivPaginationLimit { get; set; }
    }
}