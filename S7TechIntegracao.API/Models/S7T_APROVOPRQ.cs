using Newtonsoft.Json;
using System.Collections.Generic;

namespace S7TechIntegracao.API.Models
{
    public class OData_APROVOPRQ
    {
        [JsonProperty("odata.metadata")]
        public string Metadata { get; set; }
        public List<S7T_APROVOPRQ> Value { get; set; }
    }

    public class S7T_APROVOPRQ
    {
        public string Code { get; set; }
        public string U_CentroCusto { get; set; }

        [JsonProperty("S7T_APROVPRQ1Collection")]
        public List<S7T_APROVPRQ1> S7T_APROVPRQ1Collection { get; set; } = new List<S7T_APROVPRQ1>();
    }
}