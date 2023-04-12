using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace AppointmentHospital.Server.Helper
{
    public class EmailSender : IEmailSender
    {
        //https://myaccount.google.com/apppasswords
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var emailToSend = new MimeMessage();
                emailToSend.From.Add(MailboxAddress.Parse("Assos@dotnetmaster.com"));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

                ////send email
                using var emailClient = new SmtpClient();
                emailClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                emailClient.Authenticate("assossoft@gmail.com", "ugudkfncqlamuueq");
                emailClient.Send(emailToSend);
                emailClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
