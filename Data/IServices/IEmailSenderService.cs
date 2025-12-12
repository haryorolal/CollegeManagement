using CollegeManagement.Models.Emails;

namespace CollegeManagement.Data.IServices
{
    public interface IEmailSenderService
    {
        Task<SendEmailResponse> SendEmailAsync(string email, string subject, string message, string name);
    }
}
