namespace CollegeManagement.Models.Emails
{
    public class EmailServiceConfigurationModel
    {
        public string EmailFrom { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public bool EnableSsl { get; set; }
        public bool UseMailKit { get; set; }
    }
}
