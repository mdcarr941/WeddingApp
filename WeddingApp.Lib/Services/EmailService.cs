using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace WeddingApp.Lib.Services
{
    public class EmailConfiguration
    {
        public string? SmtpHost { get; init; }
        public int SmtpPort { get; init; }
        public string? SmtpUser { get; init; }
        public string? SmtpPassword { get; init; }
        public string? DefaultSenderName { get; init; }
        public string? DefaultSenderAddress { get; init; }
    }

    public class EmailService
    {
        private static string? _RsvpEmail = null;
        public static async Task<string> RsvpEmail()
        {
            if (_RsvpEmail is null)
            {
                var assembly = Assembly.GetAssembly(typeof(EmailService))
                    ?? throw new ArgumentNullException($"The assembly containing {nameof(EmailService)} could not be found.");
                const string resourceName = "WeddingApp.Lib.Services.RsvpEmail.html";
                using var resourceStream = assembly.GetManifestResourceStream(resourceName)
                    ?? throw new ArgumentNullException($"Embedded RSVP email could not be found at '{resourceName}'.");
                using var reader = new StreamReader(resourceStream);
                _RsvpEmail = await reader.ReadToEndAsync();
            }
            return _RsvpEmail;
        }

        private readonly EmailConfiguration _configuration;
        public EmailService(EmailConfiguration configuration)
            => _configuration = configuration;

        public async Task Send(MimeMessage message)
        {
            using var client = new SmtpClient();
            await client.ConnectAsync(_configuration.SmtpHost, _configuration.SmtpPort);
            if (_configuration.SmtpUser is not null)
            {
                await client.AuthenticateAsync(_configuration.SmtpUser, _configuration.SmtpPassword);
            }
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendRsvpConfirmation(InternetAddress to, InternetAddress? from = null)
        {
            if (from is null) from = new MailboxAddress(
                _configuration.DefaultSenderName,
                _configuration.DefaultSenderAddress);

            var message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = "Julia & Matthew's Wedding RSVP Confirmation";
            var builder = new BodyBuilder();
            builder.HtmlBody = await RsvpEmail();
            message.Body = builder.ToMessageBody();
            await Send(message);
        }

        public Task SendRsvpConfirmation(string? name, string? email, InternetAddress? from = null)
            => SendRsvpConfirmation(new MailboxAddress(name, email), from);
    }
}