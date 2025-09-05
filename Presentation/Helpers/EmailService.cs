using Framework.Application;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Presentation.Helpers;
public class EmailService(ILogger<EmailService> logger) : IEmailService
{
    private readonly string fromName = default;
    private readonly string Username = default;
    private readonly string Password = default;
    private readonly string smtpServer = default;
    private readonly int smtPort = default;
    public async Task Execute(string userEmail, string body, string subject)
    {
        MimeMessage message = new();
        message.From.Add(new MailboxAddress(fromName, Username));
        message.To.Add(new MailboxAddress(userEmail, userEmail));
        message.Subject = subject;
        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = body
        };
        message.Body = bodyBuilder.ToMessageBody();
        using SmtpClient client = new();
        try
        {
            await client.ConnectAsync(smtpServer, smtPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(Username, Password);
            await client.SendAsync(message);
        }
        catch (Exception e)
        {
            logger.LogError(e,
            "#EmailService.Execute.CatchException() >> Exception: " + e.Message +
            (e.InnerException != null ? $"InnerException: {e.InnerException.Message}" : string.Empty));
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}