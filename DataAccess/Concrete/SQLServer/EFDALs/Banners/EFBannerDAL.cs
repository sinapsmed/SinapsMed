using System.Globalization;
using System.Linq.Expressions;
using System.Net;
using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.Concrete.Admin;
using Entities.Concrete.Banner;
using Entities.DTOs.BannerDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Banners;

public class EFBannerDAL : Manager, IBannerDAL
{
    private readonly IRepositoryBase<Banner, Get, AppDbContext> _repo;
    private readonly IStringLocalizer<EFBannerDAL> _dalLoclizer;
    private readonly string _cultre;

    public EFBannerDAL(IRepositoryBase<Banner, Get, AppDbContext> repo, IStringLocalizer<EFBannerDAL> dalLoclizer)
    {
        _repo = repo;
        _dalLoclizer = dalLoclizer;
        _cultre = CultureInfo.CurrentCulture.Name;
    }

    public async Task<IResult> Create(Create banner, string userName)
    {
        Banner newBanner = new Banner
        {
            ImageUrl = banner.ImageUrl,
            Link = banner.Link,
            CreatedAt = DateTime.UtcNow,
            Languages = banner.LanguagesCreate
            .Select(c => new BannerLang
            {
                Title = c.Title,
                Code = c.Code,
                LinkTitle = c.LinkTitle,
                Description = c.Description
            }).ToList()
        };
        using (var context = new AppDbContext())
        {
            return await _repo.AddAsync(newBanner, context);
        }
    }

    public async Task<IResult> Delete(Guid id)
    {
        using (var context = new AppDbContext())
        {
            Banner banner = await context.Banners
                .Include(c => c.Languages)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (banner is null)
                return new ErrorResult(_dalLoclizer["bannerNotFound"], HttpStatusCode.NotFound, $"Banner Not Found Id : {id}");

            return await _repo.Remove(banner, context);
        }
    }

    public async Task<IDataResult<List<Get>>> GetAll()
    {
        using (var context = new AppDbContext())
        {
            IQueryable<Banner> banners = context.Set<Banner>().Include(c => c.Languages);

            return await _repo.GetAllAsync(banners, e => new Get
            {
                Id = e.Id,
                Title = e.Languages.FirstOrDefault(c => c.Code == _cultre).Title,
                Description = e.Languages.FirstOrDefault(c => c.Code == _cultre).Description,
                ImageUrl = e.ImageUrl,
                Link = e.Link,
                LinkTitle = e.Languages.FirstOrDefault(c => c.Code == _cultre).LinkTitle
            });
        }
    }

    public async Task<IResult> Update(Update updateGet, string userName)
    {
        using (var context = new AppDbContext())
        {
            Admin admin = await context.Admins.FirstOrDefaultAsync(c => c.Id.ToString() == userName);

            if (admin is null)
                return new ErrorResult(_dalLoclizer["unAuth"], HttpStatusCode.Unauthorized);

            var querry = context.Set<Banner>()
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
                    querry.Languages.Add(new BannerLang
                    {
                        Code = language.Code,
                        Description = language.Description,
                        Title = language.Title,
                        LinkTitle = language.LinkTitle
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
                    }
                    if (language.LinkTitle != lang.LinkTitle)
                    {
                        lang.LinkTitle = language.LinkTitle;
                    }
                }
            }

            context.Set<Banner>().Update(querry);

            querry.UpdatedAt = DateTime.UtcNow;
            querry.UpdatedBy = admin.Name;

            await context.SaveChangesAsync();

            return new SuccessResult(HttpStatusCode.OK);
        }
    }

    public async Task<IDataResult<Update>> UpdateData(Guid id)
    {
        using (var context = new AppDbContext())
        {
            var select = Selector();

            var data = await context
               .Banners
               .Include(c => c.Languages)
               .Select(select)
               .FirstOrDefaultAsync(c => c.Id == id);

            if (data is null)
                throw new DataNullException(id.ToString(), _cultre);

            return new SuccessDataResult<Update>(data: data, HttpStatusCode.OK);
        }
    }

    private Expression<Func<Banner, Update>> Selector()
    {
        return c => new Update
        {
            Id = c.Id,
            ImageUrl = c.ImageUrl,
            Link = c.Link,
            Languages = c.Languages.Select(x => new LangUpdate
            {
                Description = x.Description,
                LinkTitle = x.LinkTitle,
                Title = x.Title,
                Code = x.Code
            }).ToList()
        };
    }
}
