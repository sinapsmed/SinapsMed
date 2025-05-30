using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Faq;
using Entities.Concrete.UserEntities;
using Entities.DTOs.QuestionDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Questions
{
    public class EFQuestionsDAL : Manager, IQuestionDAL
    {
        private readonly IRepositoryBase<Question, Get, AppDbContext> _repo;
        private readonly AppDbContext _context;
        private readonly IStringLocalizer<EFQuestionsDAL> _dalLocalizer;
        private readonly string _cultre;

        public EFQuestionsDAL(IRepositoryBase<Question, Get, AppDbContext> repo, IStringLocalizer<EFQuestionsDAL> dalLocalizer, AppDbContext context)
        {
            _repo = repo;
            _dalLocalizer = dalLocalizer;
            _cultre = CultureInfo.CurrentCulture.Name;
            _context = context;
        }

        public async Task<IResult> Create(List<Create> dto)
        {
            Question question = new Question
            {
                Languages = dto.Select(c => new QuestionLang
                {
                    Code = c.Code,
                    Description = c.Description,
                    Title = c.Title,
                }).ToList()
            };

            return await _repo.AddAsync(question, _context);
        }

        public async Task<IResult> Delete(Guid id)
        {
            Question entity = await _context.Questions.FirstOrDefaultAsync(c => c.Id == id);
            if (entity is null)
                return new ErrorResult(_dalLocalizer["questionNotFound"], HttpStatusCode.NotFound, $"Question Not Found Id : {id}");

            return await _repo.Remove(entity, _context);
        }

        public async Task<IDataResult<List<Get>>> GetAll()
        {
            var questions = _context.Set<Question>()
                .Include(c => c.Languages);

            var selector = Select();

            return await _repo.GetAllAsync(questions, selector);
        }


        #region Private Methods
        private Expression<Func<Question, Get>> Select()
        {
            return c => new Get
            {
                Id = c.Id,
                Title = c.Languages.FirstOrDefault(c => c.Code == _cultre).Title,
                Description = c.Languages.FirstOrDefault(c => c.Code == _cultre).Description
            };
        }
        #endregion
    }
}