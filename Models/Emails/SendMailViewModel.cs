namespace CollegeManagement.Models.Emails
{
    public class SendMailViewModel : MessageBaseModel
    {
        public string Subject { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
    }
}
