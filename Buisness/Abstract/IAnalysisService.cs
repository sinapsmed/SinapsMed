using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Analysis.Admin;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.DTOs.AnalysisDtos.Category.Admin;

namespace Buisness.Abstract
{
    public interface IAnalysisService : IService
    {
        Task<IResult> AddList(Microsoft.AspNetCore.Http.IFormFile file, string agentMail);
        Task<IResult> Add(Create create);
        Task<IResult> AddCat(CreateCategory createCategory);
        Task<IResult> DeleteCat(Guid id);
        Task<IResult> UpdateCat(UpdateCategory updateCategory);
        Task<IDataResult<List<GetDetailedCats>>> GetDetailedCats();
        Task<IDataResult<List<GetCats>>> GetCats();
        Task<IResult> Delete(Guid id);
        Task<IResult> Update(Update update);
        Task<IDataResult<BaseDto<Get>>> GetAll(Filter filter);
        Task<IDataResult<BaseDto<DetailedGet>>> GetAllDetailed(Filter filter);
    }
}