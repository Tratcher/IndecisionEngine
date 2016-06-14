using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace IndecisionEngine.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public AuthMessageSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; }  // set only via Secret Manager

        // https://app.sendgrid.com/
        public Task SendEmailAsync(string email, string subject, string message)
        {
#if NET451
            // Plug in your email service here to send an email.
            var myMessage = new SendGrid.SendGridMessage();
            myMessage.AddTo(email);
            myMessage.From = new System.Net.Mail.MailAddress("tratcher@outlook.com", "Administrator");
            myMessage.Subject = subject;
            myMessage.Text = message;
            myMessage.Html = message;
            /*var credentials = new System.Net.NetworkCredential(
                Options.SendGridUser,
                Options.SendGridKey);*/
            // Create a Web transport for sending email.
            var transportWeb = new SendGrid.Web(Options.SendGridKey);
            // Send the email.
            if (transportWeb != null)
            {
                return transportWeb.DeliverAsync(myMessage);
            }
            else
            {
                return Task.FromResult(0);
            }
#else
            throw new NotImplementedException();
#endif
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            throw new NotImplementedException();
        }
    }
}
