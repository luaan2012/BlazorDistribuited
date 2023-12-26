using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Blazor.Api.User.Helper;

public class Emailsender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string message)
    {
        var mail = "luan_s2ju@hotmail.com";
        var pw = "UtLcE96S";

        var client = new SmtpClient("smtp-mail.outlook.com", 587)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(mail, pw)
        };

        return client.SendMailAsync(new MailMessage(from: mail, to: email, subject, message));
    }
}