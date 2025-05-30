using System.Net;
using Core.DataAccess;
using Core.Helpers;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.Concrete.Services;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.ServiceDtos.Update;
using Entities.DTOs.SpecalitiyDtos.Get;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Services.CRUD
{
    public class EFServiceUpdateDAL : ServiceAdapter
    {
        public EFServiceUpdateDAL(
            AppDbContext context,
            IStringLocalizer<ServiceAdapter> dalLocalizer,
            IRepositoryBase<Service, GetSpecality, AppDbContext> servRepo,
            IRepositoryBase<Service, GetService, AppDbContext> servAdm,
            IRepositoryBase<ServicePeriod, PeriodGetDto, AppDbContext> periodGServ,
            IRepositoryBase<ServiceCategory, GetCat, AppDbContext> catRepo) : base(context, dalLocalizer, servRepo, servAdm, periodGServ, catRepo)
        {
        }

        public override async Task<IResult> Update(ServiceUpdateGet updateGet)
        {
            var querry = _context.Set<Service>()
               .Include(c => c.Languages)
               .FirstOrDefault(c => c.Id == updateGet.Id);

            if (querry is null)
                throw new DataNullException(updateGet.Id.ToString(), _cultre);

            querry.ImageUrl = updateGet.ImageUrl ?? querry.ImageUrl;

            foreach (var language in updateGet.Languages)
            {
                var lang = querry.Languages.FirstOrDefault(c => c.Code == language.Code);
                if (lang is null)
                {
                    querry.Languages.Add(new ServiceLang
                    {
                        Code = language.Code,
                        Description = language.Description,
                        Title = language.Title,
                        Href = SeoHelper.ConverToSeo(language.Title, _cultre)
                    });
                }
                else
                {
                    if (language.Description != lang.Description)
                    {
                        lang.Description = language.Description;
                    }

                    if (language.Title != lang.Title)
                    {
                        lang.Title = language.Title;
                        lang.Href = SeoHelper.ConverToSeo(language.Title, _cultre);
                    }
                }
            }

            _context.Set<Service>().Update(querry);

            await _context.SaveChangesAsync();

            return new SuccessResult(HttpStatusCode.OK);
        }
        public override async Task<IResult> UpdatePeriod(ServicePeriodUpdateGet updateGet)
        {
            var querry = _context.Set<ServicePeriod>()
              .Include(c => c.Languages)
              .FirstOrDefault(c => c.Id == updateGet.Id);

            if (querry is null)
                throw new DataNullException(updateGet.Id.ToString(), _cultre);

            querry.Duration = updateGet.Duration;

            foreach (var language in updateGet.Languages)
            {
                var lang = querry.Languages.FirstOrDefault(c => c.Code == language.Code);
                if (lang is null)
                {
                    querry.Languages.Add(new ServicePeriodLang
                    {
                        Code = language.Code,
                        PeriodText = language.Title
                    });
                }
                else
                {
                    if (lang.PeriodText != language.Title)
                    {
                        lang.PeriodText = language.Title;
                    }
                }
            }

            _context.Set<ServicePeriod>().Update(querry);

            await _context.SaveChangesAsync();

            return new SuccessResult(HttpStatusCode.OK);
        }
        public override async Task<IResult> UpdateCategory(CategoryUpdateGet updateGet)
        {
            var querry = _context.Set<ServiceCategory>()
                .Include(c => c.Languages)
                .FirstOrDefault(c => c.Id == updateGet.Id);

            if (querry is null)
                throw new DataNullException(updateGet.Id.ToString(), _cultre);

            foreach (var language in updateGet.Languages)
            {
                var lang = querry.Languages.FirstOrDefault(c => c.Code == language.Code);
                if (lang is null)
                {
                    querry.Languages.Add(new ServiceCategoryLang
                    {
                        Code = language.Code,
                        Title = language.Title,
                        Href = SeoHelper.ConverToSeo(language.Title, _cultre)
                    });
                }
                else
                {
                    if (lang.Title != language.Title)
                    {
                        lang.Title = language.Title;
                        lang.Href = SeoHelper.ConverToSeo(language.Title, _cultre);
                    }
                }
            }

            _context.Set<ServiceCategory>().Update(querry);

            await _context.SaveChangesAsync();

            return new SuccessResult(HttpStatusCode.OK);
        }
    }
}