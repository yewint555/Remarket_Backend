using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Application.ServiceInterfaces;

namespace Application.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendOtpEmailAsync(string toEmail, string otp)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(
            _config["EmailSettings:SenderName"]!, 
            _config["EmailSettings:SenderEmail"]!));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = "Your OTP Verification Code";

        var builder = new BodyBuilder();
        builder.HtmlBody = $@"
            <div style='font-family: Arial, sans-serif; padding: 20px;'>
                <h2>Verification Code</h2>
                <p>Your OTP code is: <b style='font-size: 24px; color: #007bff;'>{otp}</b></p>
                <p>This code will expire in 1 minute.</p>
            </div>";

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try 
        {
            await smtp.ConnectAsync(
                _config["EmailSettings:SmtpServer"], 
                int.Parse(_config["EmailSettings:Port"]!), 
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _config["EmailSettings:SenderEmail"], 
                _config["EmailSettings:AppPassword"]);
            await smtp.SendAsync(email);
        }
        finally
        {
            await smtp.DisconnectAsync(true);
        }
    }
}