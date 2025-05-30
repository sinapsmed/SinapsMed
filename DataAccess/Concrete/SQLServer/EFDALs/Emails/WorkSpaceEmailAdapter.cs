using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.AuthDtos;
using Entities.DTOs.Email;

namespace DataAccess.Concrete.SQLServer.EFDALs.Emails
{
    public class WorkSpaceEmailAdapter : BaseAdapter, IWorkSpaceEmailDAL
    {
        public WorkSpaceEmailAdapter(AppDbContext context) : base(context)
        {
        }

        public virtual Task<IResult> AddAsync(string email, string password, string FullName)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<Token>> CheckPassWordAsync(string email, string pasword)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IDataResult<BaseDto<Message>>> GetMessages(string email, int page)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> SendEmailAsync(Guid workspaceEmailId, string to, string title, string content)
        {
            throw new NotImplementedException();
        }
    }
}