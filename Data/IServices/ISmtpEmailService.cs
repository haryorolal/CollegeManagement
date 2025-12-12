using CollegeManagement.Models.Emails;

namespace CollegeManagement.Data.IServices
{
    public interface ISmtpEmailService
    {
        Task<SendEmailResponse> SendMail(SendMailViewModel model, EmailServiceConfigurationModel emailConfig);
    }
}
