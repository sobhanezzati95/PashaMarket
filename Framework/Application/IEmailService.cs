namespace Framework.Application;
public interface IEmailService
{
    Task Execute(string userEmail, string body, string title);
}