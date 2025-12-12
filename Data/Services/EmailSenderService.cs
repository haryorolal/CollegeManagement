using CollegeManagement.Data.IServices;
using CollegeManagement.Models.Emails;

namespace CollegeManagement.Data.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly ISmtpEmailService _smtpEmailService;
        private readonly IConfiguration _configuration;
        public EmailSenderService(ISmtpEmailService smtpEmailService, IConfiguration configuration)
        {
            _smtpEmailService = smtpEmailService;
            _configuration = configuration;
        }
        public async Task<SendEmailResponse> SendEmailAsync(string email, string subject, string message, string name)
        {
            var from = _configuration["EmailSettings:Sender"];
            var host = _configuration["EmailSettings:MailServer"];
            var fromName = _configuration["EmailSettings:SenderName"];

            var port = Convert.ToInt32(_configuration["EmailSettings:MailPort"]);
            var userName = _configuration["EmailSettings:Sender"];
            var password = _configuration["EmailSettings:Password"];
            var enableSsl = Convert.ToBoolean(_configuration["EmailSettings:EnableSsl"]);
            var useMailKit = Convert.ToBoolean(_configuration["EmailSettings:UseMailKit"]);

            var model = new SendMailViewModel()
            {
                From = from,
                To = email,
                Subject = subject,
                Body = message,
                FromName = fromName,
                ToName = name
            };

            var serviceConfig = new EmailServiceConfigurationModel()
            {
                Host = host,
                Port = port,
                Username = userName,
                Password = password,
                EmailFrom = from,
                EnableSsl = enableSsl,
                UseMailKit = useMailKit
            };

            try
            {
                var result = await _smtpEmailService.SendMail(model, serviceConfig);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }


            throw new NotImplementedException();
        }
    }
}
