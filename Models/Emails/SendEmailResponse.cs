using System.Net;

namespace CollegeManagement.Models.Emails
{
    public class SendEmailResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccessStatusCode { get; set; }
        public HttpContent Body { get; set; }
        public HttpResponseHeader Headers { get; set; }
        public string Message { get; set; }
    }
}
