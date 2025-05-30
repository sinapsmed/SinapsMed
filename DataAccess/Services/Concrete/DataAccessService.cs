using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DataAccess.Services.Abstract;
using Entities.Concrete.Analyses;
using Entities.Concrete.Partners;
using Entities.Concrete.UserEntities;
using Entities.DTOs.Helpers;
using ExcelDataReader;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Entities.Concrete.Experts;
using Core.Utilities.Results.Abstract;
using Microsoft.AspNetCore.Identity;
using Core.Utilities.Results.Concrete.SuccessResult;
using System.Net;
using Entities.Concrete.Admin;
using Microsoft.AspNetCore.Hosting;
using Entities.Enums;
using OfficeOpenXml;
using RestSharp;
using Entities.Concrete.Clinics;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.AuthDtos;
using Entities.Concrete.AccountantEntities;
using Entities.Concrete.Staff;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Helpers;

namespace DataAccess.Services
{
    public class DataAccessService : IDAtaAccessService
    {
        private readonly IConfiguration _config;
        private readonly double _earthRadius;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _axs;
        public DataAccessService(IConfiguration config, UserManager<AppUser> userManager, IWebHostEnvironment env, Microsoft.AspNetCore.Http.IHttpContextAccessor axs)
        {
            _config = config;
            _earthRadius = 6367.45;
            _userManager = userManager;
            _env = env;
            _axs = axs;
        }
        public string AssetsPath()
        {
            var requestContext = _axs?.HttpContext?.Request;
            string scheme = requestContext?.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? requestContext?.Scheme;

            var baseUrl = $"{scheme}://{requestContext?.Host}";

            return Path.Combine(baseUrl, _env.WebRootPath, "assets");
        }
        public async Task<IDataResult<AppUser>> DecodeTokenAsync(string token)
        {
            AppUser user = await _userManager.FindByNameAsync(token);

            return new SuccessDataResult<AppUser>(user, HttpStatusCode.OK);
        }
        public double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            double lat1Rad = lat1 * Math.PI / 180;
            double lon1Rad = lon1 * Math.PI / 180;
            double lat2Rad = lat2 * Math.PI / 180;
            double lon2Rad = lon2 * Math.PI / 180;

            double dLat = lat2Rad - lat1Rad;
            double dLon = lon2Rad - lon1Rad;

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(dLon / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = _earthRadius * c;

            return distance;
        }
        public string ExpertToken(Expert expert)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,$"{expert.Id}"),
                new Claim("Full Name",expert.FullName),
                new Claim("Image",expert.PhotoUrl),
                new Claim("Superpiority",Superiority.Expert.ToString()),
                new Claim("Email",expert.Email),
                new Claim(ClaimTypes.Role, "Expert"),
                new Claim(ClaimTypes.Name, $"{expert.Id}")
            };

            return TokenHandler(1, claims);
        }
        public string GeneratePasswrod()
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string unikalCharacters = "!@#$%&*";

            Random random = new Random();

            StringBuilder password = new StringBuilder();

            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(unikalCharacters[random.Next(unikalCharacters.Length)]);

            string allChars = upperCase + lowerCase + digits;
            for (int i = 0; i < 9; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            return new string(password.ToString().ToCharArray().OrderBy(s => random.Next()).ToArray());
        }
        public string GetToken(IEnumerable<string> roles, Admin admin)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Id.ToString()),
                new Claim("Email", admin.Email),
                new Claim("Superpiority",Superiority.Admin.ToString()),
                new Claim("Full Name",admin.Name),
                new Claim("Image",admin.ProfileLogo)
            };
            foreach (var role in roles)
            {
                Claim claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }
            return TokenHandler(1, claims);
        }
        public string GetToken(IEnumerable<string> roles, AppUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Superpiority",Superiority.User.ToString()),
                new Claim("Full Name",user.FullName),
                new Claim("Image",user.ImageUrl),
                new Claim("UnikalKey",user.UnicalKey)
            };
            foreach (var role in roles)
            {
                Claim claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }

            return TokenHandler(10, claims);
        }
        public string GetToken(IEnumerable<string> roles, Clinic clinic)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,clinic.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, clinic.UnicalKey),
                new Claim("Email",clinic.Email.Email),
                new Claim("Superpiority",Superiority.Clinic.ToString()),
                new Claim("Full Name",clinic.Name),
            };
            foreach (var role in roles)
            {
                Claim claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }

            return TokenHandler(3, claims);
        }
        private string TokenHandler(double day, IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey symmetricSecurity = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:SecurityKey"]));

            SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurity, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: DateTime.UtcNow.AddDays(day));

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            return jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
        }
        public IResult CheckPasswordRequirements(string password)
        {
            if (password.Length < 8)
                return new ErrorResult("Şifrədə minimum 8 simvol olmalıdır");

            if (!password.Any(char.IsUpper))
                return new ErrorResult("Şifrədə minimum 1 böyük hərf olmalıdır");

            if (!password.Any(char.IsLower))
                return new ErrorResult("Şifrədə minimum 1 kiçik hərf olmalıdır");

            if (!password.Any(char.IsDigit))
                return new ErrorResult("Şifrədə minimum 1 rəqəm olmalıdır");

            if (!password.Any(c => "!@#$%^&*()_+-=[]{}|;':\",.<>?/`~".Contains(c)))
                return new ErrorResult("Şifrədə minimum 1 xüsusi simvol olmalıdır");

            return new SuccessResult();
        }
        public string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                // Concatenate password and salt
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                // Hash the concatenated password and salt
                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                // Concatenate the salt and hashed password for storage
                byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

                return Convert.ToBase64String(hashedPasswordWithSalt);
            }
        }
        public ExcelDataReturn<Analysis> ReadAnalysesFromExcel(string fileName, IQueryable<Partner> partners, IQueryable<AnalysisCategory> analysisCategories, IQueryable<Clinic> clinics)
        {
            List<Analysis> analyses = new List<Analysis>();
            List<Problem> problems = new List<Problem>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    int rowindex = 0;
                    while (reader.Read())
                    {
                        if (rowindex == 0)
                        {
                            rowindex++;
                            continue;
                        }
                        if (!analysisCategories.Any(c => c.Number == Convert.ToInt32(reader.GetValue(1))))
                        {
                            problems.Add(new Problem { Detail = "Daxil edilen Kateqoriya Movcud deyil və ya yanlışdır !", Row = rowindex, Column = 'B' });
                            rowindex++;
                            continue;
                        }
                        if (!partners.Any(c => c.Number == Convert.ToInt32(reader.GetValue(0))))
                        {
                            problems.Add(new Problem { Detail = "Daxil edilen Partner Movcud deyil və ya yanlışdır !", Row = rowindex, Column = 'A' });
                            rowindex++;
                            continue;
                        }
                        if (!clinics.Any(c => c.UnicalKey == reader.GetValue(2).ToString()))
                        {
                            problems.Add(new Problem { Detail = $"Daxil edilen Klinika Kodu {reader.GetValue(2)} Movcud deyil və ya yanlışdır !", Row = rowindex, Column = 'A' });
                            rowindex++;
                            continue;
                        }

                        Analysis analysis = new Analysis
                        {
                            CategoryId = analysisCategories.FirstOrDefault(c => c.Number == Convert.ToInt32(reader.GetValue(1))).Id,
                            Code = reader.GetValue(3).ToString(),
                            Name = reader.GetValue(4).ToString(),
                            Price = reader.GetDouble(5),
                            PartnerId = partners.FirstOrDefault(c => c.Number == Convert.ToInt32(reader.GetValue(0))).Id,
                            ClinicId = clinics.FirstOrDefault(c => c.UnicalKey == reader.GetValue(2).ToString()).Id
                        };

                        analyses.Add(analysis);
                        rowindex++;
                    }
                }
            }
            return new ExcelDataReturn<Analysis> { Datas = analyses, Problems = problems };
        }
        public async Task<string> ProblemsExcelData<T>(ExcelDataReturn<T> data)
            where T : class, new()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            using (ExcelPackage pkc = new ExcelPackage())
            {
                ExcelWorksheet ws = pkc.Workbook.Worksheets.Add("Problems");

                int rowStart = 2;
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                ws.Cells["A1"].Value = "Row";
                ws.Cells["B1"].Value = "Column";
                ws.Cells["C1"].Value = "Detail";

                foreach (var problem in data.Problems)
                {
                    ws.Cells[string.Format("A{0}", rowStart)].Value = problem.Row;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = problem.Column;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = problem.Detail;
                    rowStart++;
                }

                using (var range = ws.Cells[string.Format($"A{0}:C{rowStart}")])
                {
                    range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                using (var range = ws.Cells[string.Format($"A{rowStart}:C{rowStart}")])
                {
                    range.Style.Font.Bold = true;

                    ws.Row(rowStart + 1).Height = 20;
                }
                ws.Cells["A:AZ"].AutoFitColumns();

                string folderPath = Path.Combine(_env.WebRootPath, "assets", "Excels");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string filePath = Path.Combine(folderPath, "Problems.xlsx");

                await File.WriteAllBytesAsync(filePath, pkc.GetAsByteArray());

                return filePath;
            }
        }
        public async Task<bool> SendVerificationSMSAsync(string phoneNumber, string code)
        {
            string ApiUrl = _config["SMS:ApiUrl"];
            string Username = _config["SMS:UserName"];
            string ApiKey = _config["SMS:ApiKey"];
            string SenderName = _config["SMS:Sender"];
            using (var client = new HttpClient())
            {
                try
                {
                    string text = $"{code}";
                    string requestUrl = $"{ApiUrl}?user={Uri.EscapeDataString(Username)}" +
                                        $"&password={Uri.EscapeDataString(ApiKey)}" +
                                        $"&gsm={Uri.EscapeDataString(phoneNumber)}" +
                                        $"&from={Uri.EscapeDataString(SenderName)}" +
                                        $"&text={Uri.EscapeDataString(text)}";

                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    response.EnsureSuccessStatusCode();

                    string responseContent = await response.Content.ReadAsStringAsync();

                    var parts = responseContent.Split('&');
                    foreach (var part in parts)
                    {
                        var keyValue = part.Split('=');
                        if (keyValue.Length == 2 && keyValue[0] == "errno")
                        {
                            return keyValue[1] == "100";
                        }
                    }

                    using (var context = new AppDbContext())
                    {
                        var log = new Log
                        {
                            Agent = "Sinapsmed",
                            Date = DateTime.UtcNow.AddHours(4),
                            ErrorMessage = responseContent,
                            ExceptionMessage = responseContent,
                            ExecutingTime = 1,
                            IpAddres = "0.0.0.0",
                            IsSucces = false,
                            StatusCode = HttpStatusCode.BadRequest,
                            Message = "Failed",
                            StackTrace = "/DataAccesService/",
                            Path = ""
                        };
                        await context.Set<Log>().AddAsync(log);

                        await context.SaveChangesAsync();
                    }

                    return false;
                }
                catch
                {
                    return false;
                }
            }
        }
        public Token WorkSpaceToken(string email, Guid id)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Email", $"{email}"),
                new Claim(ClaimTypes.Role, "WorkSpace"),
                new Claim(ClaimTypes.NameIdentifier, $"{id}")
            };

            var token = TokenHandler(1, claims);

            return new Token
            {
                JWT = token,
            };
        }
        public string GetToken(IEnumerable<string> roles, Accountant accountant)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,accountant.Id.ToString()),
                new Claim("Superpiority",Superiority.Accountant.ToString()),
                new Claim("Full Name",accountant.Email.Title),
                new Claim("Email",accountant.Email.Email)
            };
            foreach (var role in roles)
            {
                Claim claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }

            return TokenHandler(3, claims);
        }
        public string GetToken(IEnumerable<string> roles, Support support)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,support.Id.ToString()),
                new Claim("Superpiority",Superiority.Staff.ToString()),
                new Claim("Full Name",support.WorkSpaceEmail.Title),
                new Claim("Email",support.WorkSpaceEmail.Email)
            };
            foreach (var role in roles)
            {
                Claim claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }

            return TokenHandler(3, claims);
        }
    }
}