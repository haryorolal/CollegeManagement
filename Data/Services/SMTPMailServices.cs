using CollegeManagement.Data.IServices;
using CollegeManagement.Models.Emails;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace CollegeManagement.Data.Services
{
    public class SMTPMailServices : ISmtpEmailService
    {
        private readonly SendEmailResponse _sendEmailResponse;
        public SMTPMailServices() { 
            _sendEmailResponse = new SendEmailResponse();
        }

        public async Task<SendEmailResponse> SendMail(SendMailViewModel model, EmailServiceConfigurationModel emailConfig)
        {
            if (emailConfig.UseMailKit)
            {
                MimeMessage message = new MimeMessage();
                MailboxAddress from = new MailboxAddress(model.FromName, model.From);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(model.ToName, model.To);
                message.To.Add(to);

                message.Subject = model.Subject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = model.Body;
                bodyBuilder.TextBody = model.Body;

                message.Body = bodyBuilder.ToMessageBody();

                try
                {
                    MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();
                    smtpClient.Connect(emailConfig.Host, emailConfig.Port, MailKit.Security.SecureSocketOptions.Auto);
                    smtpClient.Authenticate(emailConfig.Username, emailConfig.Password);
                    smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await smtpClient.SendAsync(message);
                    smtpClient.Disconnect(true);
                    smtpClient.Dispose();

                    _sendEmailResponse.IsSuccessStatusCode = true;
                    _sendEmailResponse.StatusCode = System.Net.HttpStatusCode.OK;
                    _sendEmailResponse.Message = "Success";
                }catch(Exception ex)
                {
                    _sendEmailResponse.IsSuccessStatusCode = false;
                    _sendEmailResponse.StatusCode = HttpStatusCode.BadRequest;
                    _sendEmailResponse.Message = $"This has an exception with this message {ex.Message}";
                }

                return _sendEmailResponse;
            }else
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(model.From);
                    mail.To.Add(model.To);
                    mail.Subject = model.Subject;
                    mail.Body = model.Body;
                    mail.IsBodyHtml = true;

                    using(System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient(emailConfig.Host, emailConfig.Port))
                    {
                        try
                        {
                            smtp.UseDefaultCredentials = false;
                            smtp.Credentials = new NetworkCredential(emailConfig.Username, emailConfig.Password);
                            smtp.EnableSsl = true;

                            smtp.Send(mail);

                            _sendEmailResponse.IsSuccessStatusCode = true;
                            _sendEmailResponse.StatusCode = System.Net.HttpStatusCode.OK;
                            _sendEmailResponse.Message = "Success";
                        }
                        catch (Exception ex)
                        {
                            _sendEmailResponse.IsSuccessStatusCode = false;
                            _sendEmailResponse.StatusCode = HttpStatusCode.BadRequest;
                            _sendEmailResponse.Message = $"This has an exception with a message, {ex.Message}";
                        }
                    }
                }
                return _sendEmailResponse;
            }
        }
    }
}
