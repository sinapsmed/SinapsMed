using Microsoft.Extensions.Localization;
using System.Net;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.Concrete.Analyses;
using Microsoft.EntityFrameworkCore;
using Core.DataAccess;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.DTOs.AnalysisDtos.Analysis;
using DataAccess.Concrete.SQLServer.DataBase;


namespace DataAccess.Concrete.SQLServer.EFDALs.Analyses.CRUD
{
    public class EFAnalysisDeleteDAL : AnalysesAdapter
    {
        public EFAnalysisDeleteDAL(
            AppDbContext context,
            IStringLocalizer<AnalysesAdapter> dalLocalizer,
            IRepositoryBase<Analysis, Get, AppDbContext> repo,
            IRepositoryBase<AnalysisCategory, GetCats, AppDbContext> repoCat
            ) : base(context, dalLocalizer, repo, repoCat)
        {
        } 

        public override async Task<IResult> Delete(Guid id)
        {
            using (var context = new AppDbContext())
            {
                Analysis analysis = await context.Analyses.FirstOrDefaultAsync(c => c.Id == id);
                if (analysis is null)
                    return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound, "Analysis Not Found");

                return await _repo.Remove(analysis, context);
            }
        }

        public override async Task<IResult> DeleteCat(Guid id)
        {
            using (var context = new AppDbContext())
            {
                AnalysisCategory category = await context.AnalysisCategories.FirstOrDefaultAsync(c => c.Id == id);
                if (category is null)
                    return new ErrorResult(_dalLocalizer["notFoundCat"], HttpStatusCode.NotFound, "Analysis Category Not Found");

                return await _repoCat.Remove(category, context);
            }
        }

    }
}