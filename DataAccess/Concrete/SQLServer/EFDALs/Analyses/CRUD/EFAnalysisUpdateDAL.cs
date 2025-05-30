using System.Net;
using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Analyses;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Category;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
namespace DataAccess.Concrete.SQLServer.EFDALs.Analyses.CRUD
{
    public class EFAnalysisUpdateDAL : AnalysesAdapter
    {
        public EFAnalysisUpdateDAL(
            AppDbContext context,
            IStringLocalizer<AnalysesAdapter> dalLocalizer,
            IRepositoryBase<Analysis, Get, AppDbContext> repo,
            IRepositoryBase<AnalysisCategory, GetCats, AppDbContext> repoCat
            ) : base(context, dalLocalizer, repo, repoCat)
        {
        }

        public override async Task<IResult> Update(Update update)
        {
            using (var context = new AppDbContext())
            {
                Analysis analysis = await context.Analyses.FirstOrDefaultAsync(c => c.Id == update.Id);
                if (analysis is null)
                    return new ErrorResult(_dalLocalizer["notFound"], HttpStatusCode.NotFound, "Analysis Not Found");

                analysis = await AnalysesSelector.AnalysisUpdatedAsync(update, analysis, context);

                return await _repo.Update(analysis, context);
            }
        }

        public override async Task<IResult> UpdateCat(UpdateCategory updateCategory)
        {
            using (var context = new AppDbContext())
            {
                AnalysisCategory category = await context.AnalysisCategories.FirstOrDefaultAsync(c => c.Id == updateCategory.Id);
                if (category is null)
                    return new ErrorResult(_dalLocalizer["notFoundCat"], HttpStatusCode.NotFound, "Analysis Category Not Found");

                category = AnalysesSelector.AnalysisCatUpdated(updateCategory, category);

                return await _repoCat.Update(category, context);
            }
        }



    }
}