using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Emails;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.DTOs.AuthDtos;
using Entities.DTOs.Email;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class WorkSpaceEmailManager : IWorkSpaceService
    {
        private readonly EmailServiceFactory _factory;
        private readonly IStringLocalizer<CommonLocalizer> _localizer;

        public WorkSpaceEmailManager(EmailServiceFactory factory, IStringLocalizer<CommonLocalizer> localizer)
        {
            _factory = factory;
            _localizer = localizer;
        }

        public async Task<IDataResult<Token>> CheckPassWordAsync(string email, string pasword)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Custom);
                var response = await dal.CheckPassWordAsync(email, pasword);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Token>(_localizer["ex"], ex);
            }
        }

        public async Task<IDataResult<BaseDto<Message>>> GetMessages(string email, int page)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await dal.GetMessages(email, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Message>>(_localizer["ex"], ex);
            }
        }

        public async Task<IResult> SendEmailAsync(Guid workspaceEmailId, string to, string title, string content)
        {
            try
            {
                var dal = _factory.GetService(ServiceFactoryType.Custom);
                var response = await dal.SendEmailAsync(workspaceEmailId, to, title, content);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_localizer["ex"], ex);
            }
        }
    }
}