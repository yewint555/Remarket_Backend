namespace Application.ServiceInterfaces;

public interface IEmailService
{
    Task SendOtpEmailAsync(string toEmail, string otp);
}