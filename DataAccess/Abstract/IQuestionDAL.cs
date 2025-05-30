using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.QuestionDtos;

namespace DataAccess.Abstract
{
    public interface IQuestionDAL : IService
    {
        Task<IResult> Create(List<Create> dto);
        Task<IDataResult<List<Get>>> GetAll();
        Task<IResult> Delete(Guid id);
    }
}