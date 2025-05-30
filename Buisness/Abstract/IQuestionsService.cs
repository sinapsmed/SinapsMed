using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.QuestionDtos;

namespace Buisness.Abstract
{
    public interface IQuestionsService : IService
    {
        Task<IResult> Create(List<Create> dto);
        Task<IDataResult<List<Get>>> GetAll();
        Task<IResult> Delete(Guid id);
    }
}