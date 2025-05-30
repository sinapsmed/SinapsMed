using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.AuthDtos;
using Entities.DTOs.Email;

namespace Buisness.Abstract
{
    public interface IWorkSpaceService
    {
        Task<IDataResult<BaseDto<Message>>> GetMessages(string email, int page);
        Task<IDataResult<Token>> CheckPassWordAsync(string email, string pasword);
        Task<IResult> SendEmailAsync(Guid workspaceEmailId, string to, string title, string content);
    }
}