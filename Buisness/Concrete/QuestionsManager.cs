using System.Net;
using Buisness.Abstract;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using Entities.DTOs.QuestionDtos;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class QuestionsManager : Manager, IQuestionsService
    {
        private readonly IStringLocalizer<CommonLocalizer> _commonLocalizer;
        private readonly IQuestionDAL _question;

        public QuestionsManager(IStringLocalizer<CommonLocalizer> commonLocalizer, IQuestionDAL question)
        {
            _commonLocalizer = commonLocalizer;
            _question = question;
        }

        public async Task<IResult> Create(List<Create> dto)
        {
            try
            {
                var response = await _question.Create(dto);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> Delete(Guid id)
        {
            try
            {
                var response = await _question.Delete(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<List<Get>>> GetAll()
        {
            try
            {
                var response = await _question.GetAll();
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Get>>(_commonLocalizer["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}