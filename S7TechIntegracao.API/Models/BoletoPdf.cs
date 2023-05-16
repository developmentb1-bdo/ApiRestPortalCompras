namespace S7TechIntegracao.API.Models
{
    public class BoletoPdf
    {
        public int codigo { get; set; } 
        public string boleto { get; set; }
        public int parcela { get; set; }
    }
}