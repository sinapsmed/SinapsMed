using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Services.Abstract;
using Entities.Concrete.Experts;
using Entities.DTOs.AuthDtos;
using System.Security.Cryptography;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text;
using Core.Utilities.Results.Concrete.SuccessResult;
using System.Web;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;

namespace DataAccess.Services.Concrete
{
    public class GoogleManager : IGoogleService
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public GoogleManager(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }

        public async Task<IDataResult<TokenPair>> AuthorizeUser(string code)
        {
            var tokenRequest = new HttpClient();
            var clientId = _config["Google:ClientId"];
            var clientSecret = _config["Google:ClientSecret"];
            var redirectUri = _config["Server:Url"] + "/auth/auth-callback";

            var response = await tokenRequest.PostAsync(_config["Google:OAuthApi"] + "/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("redirect_uri", redirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
            }));

            var tokenJson = await response.Content.ReadAsStringAsync();
            var tokenData = System.Text.Json.JsonDocument.Parse(tokenJson).RootElement;
            var accessToken = tokenData.GetProperty("access_token").GetString();

            var userInfoRequest = new HttpRequestMessage(HttpMethod.Get, $"https://www.googleapis.com/oauth2/v1/userinfo?access_token={accessToken}");

            using var httpClient = new HttpClient();
            var userInfoResponse = await httpClient.SendAsync(userInfoRequest);
            var userInfoContent = await userInfoResponse.Content.ReadAsStringAsync(); 

            if (!userInfoResponse.IsSuccessStatusCode)
            { 
                return new ErrorDataResult<TokenPair>("Google kullanıcı bilgileri alınamadı.");
            }

            var userJson = System.Text.Json.JsonDocument.Parse(userInfoContent).RootElement;

            string googleEmail = userJson.GetProperty("email").GetString();
            string googleProfile = userJson.GetProperty("picture").GetString();


            var expert = await _context.Set<Expert>()
                .FirstOrDefaultAsync(c => c.Email.ToLower() == googleEmail.ToLower());

            if (expert is null)
            {
                return new ErrorDataResult<TokenPair>($"{googleEmail} Bu Email Addresi heç bir Mütəxəssisə aid deyil");
            }

            if (!tokenData.TryGetProperty("refresh_token", out var refreshProp))
            {
                return new ErrorDataResult<TokenPair>($"{googleEmail} üçün əməliyyat uğurlu yerinə yetirilə bilmədi");
            }

            expert.PhotoUrl = googleProfile;
            expert.IsActive = true;
            expert.RefleshToken = EncryptToken(refreshProp.GetString());
 
            await _context.SaveChangesAsync();

            return new SuccessDataResult<TokenPair>();
        }

        public async Task<IDataResult<string>> CreateScheduledMeet(List<EventAttendee> attendees, DateTime start, DateTime end, string summary, string hostEmail)
        {
            var host = await _context.Set<Expert>()
                .FirstOrDefaultAsync(c => c.Email.ToLower() == hostEmail.ToLower());

            if (host is null)
                return new ErrorDataResult<string>($"{host.Email} üçün düzgün Mütəxəssis tapılmadı");

            var token = await RefreshAccessTokenAsync(DecryptToken(host.RefleshToken));

            if (token == null)
                return new ErrorDataResult<string>($"{host.Email} əvvəlcə Google üçün yetkiləndirilməlidir");

            var initializer = new BaseClientService.Initializer()
            {
                HttpClientInitializer = GoogleCredential.FromAccessToken(token),
                ApplicationName = "Sinapsmed Randevu"
            };

            var service = new CalendarService(initializer);

            var @event = new Event
            {
                Summary = summary,
                Start = new EventDateTime
                {
                    DateTime = start,
                    TimeZone = "Asia/Baku"
                },
                Attendees = attendees,
                End = new EventDateTime
                {
                    DateTime = end,
                    TimeZone = "Asia/Baku"
                },
                ConferenceData = new ConferenceData
                {
                    CreateRequest = new CreateConferenceRequest
                    {
                        RequestId = Guid.NewGuid().ToString(),
                        ConferenceSolutionKey = new ConferenceSolutionKey
                        {
                            Type = "hangoutsMeet"
                        }
                    }
                }
            };

            var request = service.Events.Insert(@event, "primary");
            request.ConferenceDataVersion = 1;

            var created = await request.ExecuteAsync();
            var link = created.ConferenceData.EntryPoints?[0]?.Uri;

            return new SuccessDataResult<string>(data: link);
        }

        public async Task<string> RefreshAccessTokenAsync(string refreshToken)
        {
            var tokenRequest = new HttpClient();
            var clientId = _config["Google:ClientId"];
            var clientSecret = _config["Google:ClientSecret"];

            var response = await tokenRequest.PostAsync("https://oauth2.googleapis.com/token", new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
            }));

            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var json = System.Text.Json.JsonDocument.Parse(content).RootElement;
            var newAccessToken = json.GetProperty("access_token").GetString();

            return newAccessToken;
        }

        public IDataResult<string> GoogleLogin(string expertEmail)
        {
            var redirectUri = _config["Server:Url"] + $"/auth/auth-callback";
            var clientId = _config["Google:ClientId"];
            var scope = string.Join(" ", new[]
            {
                "https://www.googleapis.com/auth/calendar",
                "https://www.googleapis.com/auth/userinfo.email",
                "https://www.googleapis.com/auth/userinfo.profile"
            });
            var state = Guid.NewGuid().ToString();

            var authUrl = $"{_config["Google:Acconut"]}/o/oauth2/v2/auth?" +
                          $"client_id={clientId}&" +
                          $"redirect_uri={HttpUtility.UrlEncode(redirectUri)}&" +
                          $"response_type=code&" +
                          $"scope={HttpUtility.UrlEncode(scope)}&" +
                          $"access_type=offline&" +
                          $"prompt=consent&" +
                          $"login_hint={expertEmail}&" +
                          $"state={state}";

            return new SuccessDataResult<string>(data: authUrl);
        }

        public string EncryptToken(string token)
        {
            string key = _config["Encryption:Key"];
            string iv = _config["Encryption:IV"];

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(token);
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string DecryptToken(string encryptedToken)
        {
            string key = _config["Encryption:Key"];
            string iv = _config["Encryption:IV"];

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = Encoding.UTF8.GetBytes(iv);

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(Convert.FromBase64String(encryptedToken)))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}