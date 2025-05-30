using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Emails;
using Entities.DTOs.Email;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD
{
    public class EFWorkSpaceEmailReadDAL : WorkSpaceEmailAdapter
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryBase<WorkSpaceEmail, Message, AppDbContext> _repository;
        private const string PREFIX = "emails/";
        public EFWorkSpaceEmailReadDAL(AppDbContext context, IConfiguration configuration, IRepositoryBase<WorkSpaceEmail, Message, AppDbContext> repository) : base(context)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public override async Task<IDataResult<BaseDto<Message>>> GetMessages(string email, int page)
        {
            var mesajlar = new List<Message>();

            var credentials = new BasicAWSCredentials(_configuration["AWS:BAccessKey"], _configuration["AWS:BSecretKey"]);

            using var client = new AmazonS3Client(credentials, Amazon.RegionEndpoint.USEast1);

            var listRequest = new ListObjectsV2Request
            {
                BucketName = _configuration["AWS:BucketName"],
                Prefix = PREFIX + email
            };

            var listResponse = await client.ListObjectsV2Async(listRequest);

            foreach (var obj in listResponse.S3Objects.OrderByDescending(c => c.LastModified).Skip((page - 1) * 20).Take(20))
            {
                var getRequest = new GetObjectRequest
                {
                    BucketName = _configuration["AWS:BucketName"],
                    Key = obj.Key
                };

                using var response = await client.GetObjectAsync(getRequest);

                using var memoryStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                try
                {
                    var mimeMessage = await MimeMessage.LoadAsync(memoryStream);

                    var user = mimeMessage.From?.ToString().Split(' ')[0].Trim('"');
                    var senderEmail = mimeMessage.From?.ToString().Split(' ').FirstOrDefault(c => c.Contains("@"))?.Trim('<', '>');

                    mesajlar.Add(new Message
                    {
                        Email = senderEmail ?? "Bilinmir",
                        Title = mimeMessage.Subject ?? "(konusuz)",
                        Date = mimeMessage.Date,
                        Content = mimeMessage.TextBody ?? mimeMessage.HtmlBody ?? "(boş)",
                        User = user ?? "Bilinmir"
                    });
                }
                catch (FormatException ex)
                {
                    return new ErrorDataResult<BaseDto<Message>>($"MIME parse hatası: {obj.Key} - {ex.Message}");
                }
                catch (Exception ex)
                {
                    return new ErrorDataResult<BaseDto<Message>>($"Genel hata: {obj.Key} - {ex.Message}");
                }
            }

            var data = new BaseDto<Message>
            {
                Data = mesajlar,
                CurrentPage = page,
                PageCount = (int)Math.Ceiling((double)listResponse.S3Objects.Count() / 20)
            };

            return new SuccessDataResult<BaseDto<Message>>(data);

        }

    }
}