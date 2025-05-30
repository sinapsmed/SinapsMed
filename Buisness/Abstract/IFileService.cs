using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.Enums.File;

namespace Buisness.Abstract
{
    public interface IFileService : IService
    {
        Task<IDataResult<string>> Create(Microsoft.AspNetCore.Http.IFormFile file, FileCategory category, FileType type);
        Task<IResult> Delete(string url);
        Task<IDataResult<string>> Update(string url, Microsoft.AspNetCore.Http.IFormFile file, FileCategory category);
    }
}