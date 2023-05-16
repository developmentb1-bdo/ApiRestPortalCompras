using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class AprovadorEsboco
    {
        public int CodAprovador { get; set; } = -1;
        public string Aprovador { get; set; } = string.Empty;
        public string CentroCusto { get; set; } = string.Empty;
        public string EmailEnviado { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Remarks { get; set; }
        public string DataHora { get; set; }
        public AprovadorEsboco()
        {

        }
    }
}