using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResult;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace Core.Helpers.Concrete
{
    public class EmailManager : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly string _accessKey;
        private readonly string _secretKey;
        private static readonly RegionEndpoint _region = RegionEndpoint.EUNorth1;

        public EmailManager(IConfiguration config)
        {
            _config = config;
            _accessKey = _config["AWS:AccessKey"];
            _secretKey = _config["AWS:SecretKey"];
        }
        public bool IsValidEmail(string email)
        {
            email = email.Trim();
            if (string.IsNullOrEmpty(email)) return false;
            var pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            Regex regex = new(pattern);
            return regex.IsMatch(email);
        }
        public async Task<IResult> SendEmailAsync(string to, string subject, string body, string from, string? attachmentPath)
        {
            BackgroundJob.Enqueue(() => SendEmailJobAsync(new List<string> { to }, subject, body, from));

            return new SuccessResult();
        }


        public async Task<IResult> SendBulkEmailAsync(List<string> to, string subject, string body, string from, string? attachment = null)
        {
            BackgroundJob.Enqueue(() => SendEmailJobAsync(to, subject, body, from));

            return new SuccessResult();
        }

        public async Task SendEmailJobAsync(List<string> to, string subject, string body, string from)
        {
            using var client = new AmazonSimpleEmailServiceClient(_accessKey, _secretKey, _region);

            var source = _config[$"AWS:{from}"];

            if (from.Contains("@"))
            {
                source = from;
            }

            var sendRequest = new SendEmailRequest
            {
                Source = source,
                Destination = new Destination
                {
                    ToAddresses = to
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content(body)
                    }
                }
            };
 
            var response = await client.SendEmailAsync(sendRequest);
        }

    }
}
