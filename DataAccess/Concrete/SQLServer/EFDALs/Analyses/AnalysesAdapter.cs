using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.Concrete.Analyses;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Analysis.Admin;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.DTOs.AnalysisDtos.Category.Admin;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Analyses
{
    public class AnalysesAdapter : BaseAdapter, IAnalysisDAL
    {
        protected readonly IStringLocalizer<AnalysesAdapter> _dalLocalizer;
        protected readonly IRepositoryBase<Analysis, Get, AppDbContext> _repo;
        protected readonly IRepositoryBase<AnalysisCategory, GetCats, AppDbContext> _repoCat;
        public AnalysesAdapter(AppDbContext context, IStringLocalizer<AnalysesAdapter> dalLocalizer, IRepositoryBase<Analysis, Get, AppDbContext> repo, IRepositoryBase<AnalysisCategory, GetCats, AppDbContext> repoCat) : base(context)
        {
            _dalLocalizer = dalLocalizer;
            _repo = repo;
            _repoCat = repoCat;
        }

        public virtual Task<IResult> Add(Create create)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> AddCat(CreateCategory createCategory)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> AddList(Microsoft.AspNetCore.Http.IFormFile file, string agentMail)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> Delete(Guid id)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> DeleteCat(Guid id)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<Get>>> GetAll(Filter filter)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<DetailedGet>>> GetAllDetailed(Filter filter)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<List<GetCats>>> GetCats()
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<List<GetDetailedCats>>> GetDetailedCats()
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> Update(Update update)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> UpdateCat(UpdateCategory updateCategory)
        {
            throw new SystemNotWorkingException();
        }
    }
}