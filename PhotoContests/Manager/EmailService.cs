using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using PhotoContests.Configurations;
using PhotoContests.Models;

namespace PhotoContests.Manager
{
    public class EmailService: IEmailService
    {
        EmailSettings emailSettings = null;

        public EmailService(IOptions<EmailSettings> options)
        {
            emailSettings = options.Value;
        }

        public bool SendEmail(DetailsEmail details)
        {
            try
            {
                MimeMessage messageEmail = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(emailSettings.Name, emailSettings.EmailId);
                messageEmail.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(details.EmailToName, details.EmailToId);
                messageEmail.To.Add(emailTo);

                messageEmail.Subject = details.EmailTitle;

                BodyBuilder emailBodyBuilder = new BodyBuilder();
                //emailBodyBuilder.TextBody = details.EmailBody;
                emailBodyBuilder.HtmlBody = details.EmailBody;
                messageEmail.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                emailClient.Connect("smtp.gmail.com", 465, true);
                emailClient.Authenticate(emailSettings.EmailId, emailSettings.AppPassword);
                emailClient.Send(messageEmail);
                emailClient.Disconnect(true);
                emailClient.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
