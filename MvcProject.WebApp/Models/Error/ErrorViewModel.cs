namespace MvcProject.WebApp.Models.Error
{
    public class ErrorViewModel
    {
        public string Message { get; set; }
        public int? StatusCode { get; set; }
        public ErrorUrlViewModel ErrorUrl { get; set; }
    }
}