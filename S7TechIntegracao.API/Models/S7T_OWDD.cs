using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S7TechIntegracao.API.Models
{
    public class S7T_OWDD
    {
        public string Code { get; set; }
        public int U_DocEntry { get; set; }
        public DateTime U_DocDate { get; set; }
        public string U_Status { get; set; } = "W";
        public string U_IsDraft { get; set; } = "Y";
        public int U_DraftEntry { get; set; }
        public string U_ObjType { get; set; }
        public int U_MaxReqr { get; set; }
        public int U_MaxRejReqr { get; set; }
        
        [JsonProperty("S7T_WDD1Collection")]
        public List<S7T_WDD1> Aprovadores { get; set; } = new List<S7T_WDD1>();

        public S7T_OWDD()
        {

        }
    }
}