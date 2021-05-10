using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeddingApp.Lib.Data;

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
        private static string? _LinkEmail = null;
        private static readonly Regex EmailVariableRgx
            = new Regex(@"\${([^}]+)}", RegexOptions.Compiled);

        public static async Task<string> RsvpEmail(EmailConfirmationCode confirmation)
        {
            if (_RsvpEmail is null)
            {
                _RsvpEmail = await ResourceService.GetResourceString(
                    "WeddingApp.Lib.Services.RsvpEmail.html");
            }
            return EmailVariableRgx.Replace(
                _RsvpEmail,
                match => "Code" == match.Groups[1].Value ? confirmation.Code.ToString() : match.Groups[0].Value);
        }

        public static async Task<string> MeetingLinkEmail()
        {
            if (_LinkEmail is null)
            {
                _LinkEmail = await ResourceService.GetResourceString(
                    "WeddingApp.Lib.Services.MeetingLinkEmail.html");
            }
            return _LinkEmail;
        }

        private readonly EmailConfiguration _configuration;
        private readonly WeddingDbContext _weddingDb;

        public EmailService(
            EmailConfiguration configuration,
            WeddingDbContext weddingDb)
        {
            _configuration = configuration;
            _weddingDb = weddingDb;
        }

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

        public async Task Send(
            string recipientName,
            string recipientAddr,
            string subject,
            string body,
            InternetAddress? from = null)
        {
            if (from is null) from = new MailboxAddress(
                _configuration.DefaultSenderName,
                _configuration.DefaultSenderAddress);

            var to = new MailboxAddress(recipientName, recipientAddr);
            var message = new MimeMessage();
            message.From.Add(from);
            message.To.Add(to);
            message.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();
            await Send(message);
        }

        public async Task SendRsvpConfirmation(string recipientName, string recipientAddr)
            => await Send(
                recipientName,
                recipientAddr,
                "Julia & Matthew's Wedding RSVP Confirmation",
                await RsvpEmail(await _weddingDb.EmailConfirmationCode(recipientAddr)));

        public async Task SendMeetingLink(string recipientName, string recipientAddr)
            => await Send(
                recipientName,
                recipientAddr,
                "Julia & Matthew's Wedding Link",
                await MeetingLinkEmail());
    }
}