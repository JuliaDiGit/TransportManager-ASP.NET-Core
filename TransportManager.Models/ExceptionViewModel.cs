namespace TransportManager.Models
{
    public class ExceptionViewModel
    { 
        public string ClassName { get; set; }
        public string Message { get; set; }
        public ExceptionViewModel InnerException { get; set; }
    }
}
