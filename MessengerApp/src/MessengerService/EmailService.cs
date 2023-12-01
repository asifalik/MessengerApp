using MessengerDomain.Config;
using MessengerDomain.Email;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace MessengerService
{
    public class EmailService : IEmailService
    {
        #region CONSTRUCTOR, PRIVATE VARIABLES
        private readonly SMTPConfig _smtpConfig;
        private readonly NotificationsTimeConfig _emailTimeConfig = new NotificationsTimeConfig();

        public EmailService(IOptions<SMTPConfig> smtpConfig, IOptions<NotificationsTimeConfig> emailTimeConfig)
        {
            _smtpConfig = smtpConfig.Value;
            _emailTimeConfig = emailTimeConfig.Value;
        } 
        #endregion

        #region PUBLIC METHODS
        public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest sendEmailRequest)
        {
            try
            {
                if (ValidEmailTimeSpan())
                {
                    using (var smtpClient = new SmtpClient(_smtpConfig.Host, _smtpConfig.Port))
                    {
                        smtpClient.Credentials = new System.Net.NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password);
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.EnableSsl = true;
                        smtpClient.Timeout = _smtpConfig.TimeOut;


                        var message = GetMailMessage(sendEmailRequest);
                        await smtpClient.SendMailAsync(message);
                        return new SendEmailResponse { Status = true, Message = "Email sent successfully" };
                    }
                }
                else {
                    return new SendEmailResponse { Status = false, Message = "Timespan Validation Failed." };
                }
            }
            catch (Exception ex)
            {
                return new SendEmailResponse { Status = false, Message = ex.Message, Exception = ex.InnerException?.Message };
            }
        }
        #endregion

        #region PRIVATE METHODS

        private bool ValidEmailTimeSpan() {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;

            if (currentTime.TotalMinutes >= _emailTimeConfig.From.TotalMinutes && currentTime.TotalMinutes <= _emailTimeConfig.To.TotalMinutes) 
            {
                return true;
            }
            return false;
        }
        private MailMessage GetMailMessage(SendEmailRequest sendEmailRequest)
        {
            var mail = new MailMessage()
            {
                Subject = "Customer Awareness Program Title",
                Body = "Customer Awareness Program Body",
                From = new MailAddress(_smtpConfig.FromMailId)
            };
            mail.To.Add(new MailAddress(sendEmailRequest.RecipientEmailId));

            return mail;
        } 
        #endregion
    }
}