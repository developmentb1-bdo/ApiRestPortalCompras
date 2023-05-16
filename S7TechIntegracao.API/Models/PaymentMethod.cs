using Newtonsoft.Json;

namespace S7TechIntegracao.API.Models
{
    public class PaymentMethod
    {
        [JsonProperty(PropertyName = "GroupNum")]
        public int GroupNumber { get; set; }
        [JsonProperty(PropertyName = "PymntGroup")]
        public string PaymentTermsGroupName { get; set; }
    }
}