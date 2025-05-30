using System.Globalization;
using System.Net;
using Buisness.Abstract;
using Buisness.Extentions;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Entities.Enums.File;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class FileManager : Manager, IFileService
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _axs;
        private readonly IWebHostEnvironment _env;
        private readonly IStringLocalizer<CommonLocalizer> _commonLoclizer;
        private readonly string cultre;

        public FileManager(IHttpContextAccessor axs, IWebHostEnvironment env, IStringLocalizer<CommonLocalizer> commonLoclizer)
        {
            _axs = axs;
            _env = env;
            _commonLoclizer = commonLoclizer;
            cultre = CultureInfo.CurrentCulture.Name;
        }

        public async Task<IDataResult<string>> Create(IFormFile file, FileCategory category, FileType type)
        {
            try
            {
                string url = string.Empty;

                string nextPath = Enum.GetName(category);

                var requestContext = _axs?.HttpContext?.Request;
                string scheme = requestContext?.Headers["X-Forwarded-Proto"].FirstOrDefault() ?? requestContext?.Scheme;

                string folderDir = Path.Combine(_env.WebRootPath, "assets", "Files", nextPath);

                if (!Directory.Exists(folderDir))
                    Directory.CreateDirectory(folderDir);

                var baseUrl = $"{scheme}://{requestContext?.Host}/assets/Files/" + nextPath + "/";

                if (type == FileType.Image)
                    url = baseUrl + await file.CreateFileAsync(_env, "assets", "Files", nextPath);
                else if (type == FileType.Other)
                {
                    var folder = Path.Combine(_env.WebRootPath, "assets", "Files", nextPath);
                    if (Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileExtension = Path.GetExtension(file.FileName).ToLower();

                    var filePath = Path.Combine(folder, $"{Guid.NewGuid()}{fileExtension}");

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {   
                        await file.CopyToAsync(stream);
                    }   
                }

                return new SuccessDataResult<string>(url, "Ok", HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>(_commonLoclizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<Core.Utilities.Results.Abstract.IResult> Delete(string url)
        {
            try
            {
                string[] paths = url.Split("/");
                string fileName = paths.Last();
                string lastFolder = paths[^2];

                FileExtention.DeleteFile(fileName, _env, "assets", "Files", lastFolder);

                return new SuccessResult("Success", HttpStatusCode.OK);
            }
            catch (System.Exception ex)
            {
                return new ErrorResult(_commonLoclizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<string>> Update(string url, IFormFile file, FileCategory category)
        {
            try
            {
                var result = await Delete(url);
                if (result.Success)
                {
                    var create = await Create(file, category, FileType.Image);
                    if (create.Success) return create;
                }
                return new ErrorDataResult<string>(result.Message, HttpStatusCode.BadRequest, "Problem Occured When attemping delete the file");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<string>(_commonLoclizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}