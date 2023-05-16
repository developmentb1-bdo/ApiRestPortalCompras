using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class S7T_WDD1
    {
        public string Code { get; set; } = null;
        public int LineId { get; set; }
        public object U_UserID { get; set; } = null;
        public string U_Status { get; set; } = "W";
        public string U_Remarks { get; set; } = "";
        public int U_empID { get; set; }
        public string U_CentroCusto { get; set; }
        [JsonProperty("U_EmailEnv")]
        public string U_EmailEnv { get; set; } = "N";
        public DateTime? U_DataAprovacao { get; set; }
        public DateTime? U_HoraAprovacao { get; set; }

        public S7T_WDD1()
        {

        }
    }
}