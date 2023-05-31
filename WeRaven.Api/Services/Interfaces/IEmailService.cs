namespace WeRaven.Api.Services.Interfaces
{
    public interface IEmailService
    {
        Task CompileAsync<T>(string templateName, T model);
        bool Send(string toName, string toEmail, string subject, string fromName = "Equipe WeRaven", string fromEmail = "no-reply@weraven.net");
    }
}
