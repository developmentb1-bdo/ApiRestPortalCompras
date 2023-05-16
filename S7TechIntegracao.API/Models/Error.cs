namespace S7TechIntegracao.API.Models {
    public class Error {
        public int code { get; set; }
        public ErrorMessage message { get; set; }
    }

    public class ErrorMessage {
        public string lang { get; set; }
        public string value { get; set; }
    }

    public class ErrorRoot {
        public Error error { get; set; }
    }
}