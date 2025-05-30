using Core.Utilities.Results.Abstract;
using Entities.DTOs.AuthDtos;
using Google.Apis.Calendar.v3.Data;

namespace DataAccess.Services.Abstract
{
    public interface IGoogleService
    {
        Task<IDataResult<TokenPair>> AuthorizeUser(string code);
        IDataResult<string> GoogleLogin(string expertEmail);
        Task<IDataResult<string>> CreateScheduledMeet(List<EventAttendee> attendees, DateTime start, DateTime end, string summary, string hostEmail);
    }
}