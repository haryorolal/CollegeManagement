namespace CollegeManagement.Models.Emails
{
    public class MessageBaseModel
    {
        public string From { get; set; }

        public string To { get; set; }
        public List<string> MultipleTo { get; set; }
        public string Body { get; set; }
        public string ErrorMessage { get; set; }
        public List<object> FormControls { get; set; }
    }
}
