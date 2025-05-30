using System.Linq.Expressions;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.Analyses;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AnalysisDtos.Analysis.Admin;
using Entities.DTOs.AnalysisDtos.Category;
using Entities.DTOs.AnalysisDtos.Category.Admin;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.SQLServer.EFDALs.Analyses
{
    public class AnalysesSelector
    {

        public static Expression<Func<Analysis, DetailedGet>> SelectDetailedAnalyses()
        {
            return c => new DetailedGet
            {
                Code = c.Code,
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Discounted = double.Round(c.Price - ((double)c.Price * c.Category.DiscountedPercent / 100), 2),
                PartnerName = c.Partner.Name,
                PartnerId = c.Partner.Id,
                CategoryId = c.Category.Id,
                CategoryName = c.Category.Name
            };
        }
        public static Expression<Func<Analysis, Get>> SelectAnalyses()
        {
            return c => new Get
            {
                Code = c.Code,
                Id = c.Id,
                Name = c.Name,
                Price = c.Price,
                Discounted = double.Round(c.Price - ((double)c.Price * c.Category.DiscountedPercent / 100), 2)
            };
        }

        public static Func<Get, bool> ConditionAnalysis(string search)
        {
            return c => c.Name.ToLower().Contains(search.ToLower()) || c.Code.ToLower().Contains(search.ToLower());
        }

        public static AnalysisCategory AnalysisCatUpdated(UpdateCategory update, AnalysisCategory category)
        {
            category.Name = update.Name ?? category.Name;
            category.DiscountedPercent = update.DiscountedPercent;
            return category;
        }

        public static Expression<Func<AnalysisCategory, GetCats>> SelectCats()
        {
            return c => new GetCats
            {
                Id = c.Id,
                Name = c.Name
            };
        }

        public static Expression<Func<AnalysisCategory, GetDetailedCats>> SelectDetailedCats()
        {
            return c => new GetDetailedCats
            {
                Id = c.Id,
                Name = c.Name,
                AnalysesCount = c.Analyses.Count(),
                DiscountedPercent = c.DiscountedPercent,
                Number = c.Number
            };
        }

        public static async Task<Analysis> AnalysisUpdatedAsync(Update update, Analysis analysis, AppDbContext context)
        {
            analysis.Name = update.Name ?? analysis.Name;
            analysis.Code = update.Code ?? analysis.Code;

            var category = await context.AnalysisCategories.FirstOrDefaultAsync(c => c.Id == update.CategoryId);

            if (category is not null)
                analysis.CategoryId = category.Id;

            var partner = await context.Partners.FirstOrDefaultAsync(c => c.Id == update.PartnerId);

            if (partner is not null)
                analysis.PartnerId = partner.Id;

            analysis.Price = update.Price is 0 ? analysis.Price : update.Price;

            return analysis;
        }
    }
}