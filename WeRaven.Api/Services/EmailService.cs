using Amethyst;
using System.Net.Mail;
using System.Net;
using WeRaven.Api.Services.Interfaces;
using WeRaven.Api.Models.Helpers;
using WeRaven.Api.Tools;

namespace WeRaven.Api.Services
{
    public class EmailService : IEmailService
    {
        private string _body = "";
        private readonly MailSettings _mailSettings;
        public EmailService(IConfiguration configuration)
        {
            _mailSettings = configuration.GetSection("MailSettings").Get<MailSettings>() ?? throw new Exception("Can't get Mail Settings");
        }
        public async Task CompileAsync<T>(string templateName, T model)
        {
            var appRoot = EnvTool.IsDebug() ? Directory.GetCurrentDirectory() : AppDomain.CurrentDomain.BaseDirectory;
            var fileTemplate = await File.ReadAllTextAsync(Path.Combine(appRoot, "Templates", $"{templateName}.html"));
            var mailCompiler = new MailCompiler<T>(fileTemplate);
            _body = mailCompiler.Compile(model);
        }
        public bool Send(
        string toName,
        string toEmail,
        string subject,
        string fromName = "Equipe WeRaven", 
        string fromEmail = "no-reply@weraven.net")
        {
            var smtpClient = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
            {
                Credentials = new NetworkCredential(_mailSettings.User, _mailSettings.Pass),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true
            };
            var mail = new MailMessage
            {
                From = new MailAddress(fromEmail, fromName)
            };
            mail.To.Add(new MailAddress(toEmail, toName));
            mail.Subject = subject;
            mail.Body = _body;
            mail.IsBodyHtml = true;
            try
            {
                smtpClient.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
