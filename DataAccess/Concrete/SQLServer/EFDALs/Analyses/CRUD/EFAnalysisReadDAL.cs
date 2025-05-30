using Core.DataAccess;
using Core.Entities;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Analyses;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Analysis.Admin;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.DTOs.AnalysisDtos.Category.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Analyses.CRUD
{
    public class EFAnalysisReadDAL : AnalysesAdapter
    {
        private readonly IRepositoryBase<Analysis, DetailedGet, AppDbContext> _detailedRepo;
        private readonly IRepositoryBase<AnalysisCategory, GetDetailedCats, AppDbContext> _detailedCatRepo;

        public EFAnalysisReadDAL(
            AppDbContext context, IStringLocalizer<AnalysesAdapter> dalLocalizer,
            IRepositoryBase<Analysis, Get, AppDbContext> repo,
            IRepositoryBase<AnalysisCategory, GetCats, AppDbContext> repoCat,
            IRepositoryBase<Analysis, DetailedGet, AppDbContext> detailedRepo,
            IRepositoryBase<AnalysisCategory, GetDetailedCats, AppDbContext> detailedCatRepo
            ) : base(context, dalLocalizer, repo, repoCat)
        {
            _detailedRepo = detailedRepo;
            _detailedCatRepo = detailedCatRepo;
        }

        public async override Task<IDataResult<BaseDto<DetailedGet>>> GetAllDetailed(Filter filter)
        {
            IQueryable<Analysis> analyses = _context.Analyses
                .Include(c => c.Partner)
                .Include(c => c.Category)
                .Where(c => c.ClinicId == filter.ClinicId)
                .AsQueryable();

            if (filter.CategoryIds?.Any() == true)
            {
                analyses = analyses.Where(a => filter.CategoryIds.Contains(a.CategoryId));
            }

            Func<Get, bool> searchPredicate = null;

            if (!string.IsNullOrEmpty(filter.Search))
            {
                searchPredicate = AnalysesSelector.ConditionAnalysis(filter.Search);
            }

            DtoFilter<Analysis, DetailedGet> dtoFilter = new()
            {
                Limit = filter.Limit,
                Page = filter.Page,
                Selector = AnalysesSelector.SelectDetailedAnalyses()
            };

            return await _detailedRepo.GetAllAsync(analyses, dtoFilter, searchPredicate);
        }

        public override async Task<IDataResult<BaseDto<Get>>> GetAll(Filter filter)
        {
            IQueryable<Analysis> analyses = _context.Analyses
                .Include(c => c.Clinic)
                .AsQueryable();

            if (filter.CategoryIds?.Any() == true)
            {
                analyses = analyses.Where(a => filter.CategoryIds.Contains(a.CategoryId));
            }

            analyses = analyses.Where(a => a.ClinicId == filter.ClinicId);

            Func<Get, bool> searchPredicate = null;

            if (!string.IsNullOrEmpty(filter.Search))
            {
                searchPredicate = AnalysesSelector.ConditionAnalysis(filter.Search);
            }

            DtoFilter<Analysis, Get> dtoFilter = new DtoFilter<Analysis, Get>
            {
                Limit = filter.Limit,
                Page = filter.Page,
                Selector = AnalysesSelector.SelectAnalyses()
            };

            return await _repo.GetAllAsync(analyses, dtoFilter, searchPredicate);

        }

        public override async Task<IDataResult<List<GetCats>>> GetCats()
        {
            var cats = _context.Set<AnalysisCategory>();

            var selector = AnalysesSelector.SelectCats();

            return await _repoCat.GetAllAsync(cats, selector);
        }

        public override async Task<IDataResult<List<GetDetailedCats>>> GetDetailedCats()
        {
            var cats = _context.Set<AnalysisCategory>();

            var selector = AnalysesSelector.SelectDetailedCats();

            return await _detailedCatRepo.GetAllAsync(cats, selector);
        }

    }
}