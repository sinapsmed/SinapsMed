using System.Globalization;
using System.Linq.Expressions;
using Entities.Concrete.Services;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.ServiceDtos.Update;
using Entities.DTOs.SpecalitiyDtos.Get;

namespace DataAccess.Concrete.SQLServer.EFDALs.Services
{
    public class ServiceSelector
    {

        public static Expression<Func<ServiceCategory, CategoryUpdateGet>> UpdateCategoryGet()
        {
            return c => new CategoryUpdateGet
            {
                Id = c.Id,
                Languages = c.Languages.Select(x => new CategoryUpdateLanguageGet
                {
                    Code = x.Code, 
                    Title = x.Title
                }).ToList()
            };
        }
        public static Expression<Func<ServicePeriod, ServicePeriodUpdateGet>> UpdatePeriodGet()
        {
            return c => new ServicePeriodUpdateGet
            {
                Id = c.Id,
                Duration = c.Duration,
                ServiceId = c.ServiceId,
                Languages = c.Languages.Select(x => new UpdatePeriodLanguageGet
                {
                    Code = x.Code,
                    Title = x.PeriodText
                }).ToList()
            };
        }
        public static Expression<Func<Service, ServiceUpdateGet>> UpdateGet()
        {
            return c => new ServiceUpdateGet
            {
                CategoryId = c.CategoryId,
                Id = c.Id,
                ImageUrl = c.ImageUrl,
                Languages = c.Languages.Select(x => new UpdateLanguageGet
                {
                    Code = x.Code,
                    Description = x.Description, 
                    Title = x.Title
                }).ToList()
            };
        }
        public static Expression<Func<ServiceCategory, GetCat>> Categories(string _cultre)
        {
            return c => new GetCat
            {
                Id = c.Id,
                Title = c.Languages.FirstOrDefault(c => c.Code == _cultre).Title
            };
        }

        public static Expression<Func<ServicePeriod, PeriodGetDto>> Period(string _cultre)
        {
            return c => new PeriodGetDto
            {
                Id = c.Id,
                Title = c.Languages.FirstOrDefault(c => c.Code == _cultre).PeriodText,
                Duration = c.Duration,
            };
        }

        public static Expression<Func<Service, GetService>> Services(string _cultre)
        {
            return c => new GetService
            {
                Id = c.Id,
                Title = c.Languages.FirstOrDefault(c => c.Code == _cultre).Title,
                Description = c.Languages.FirstOrDefault(c => c.Code == _cultre).Description,
                ImageUrl = c.ImageUrl
            };
        }

        public static Expression<Func<Service, GetSpecality>> AllService(string _cultre)
        {
            return c => new GetSpecality
            {
                Id = c.Id,
                Title = c.Languages.FirstOrDefault(c => c.Code == _cultre).Title
            };
        }
        public static Expression<Func<ServiceCategory, GetHeader>> Header(string cultre)
        {
            return c => new GetHeader
            {
                Name = c.Languages.FirstOrDefault(c => c.Code == cultre).Title,
                SubRoutes = c.Services.Select(x => new SubHeader
                {
                    Name = x.Languages.FirstOrDefault(c => c.Code == cultre).Title,
                    Href = $"/services/{c.Languages.FirstOrDefault(c => c.Code == cultre).Href}/{x.Languages.FirstOrDefault(c => c.Code == cultre).Href}?id={x.Id}",
                }).ToList()
            };
        }

        public static Detail DetailFuc(Service service, string _cultre)
        {
            ServiceLang lang = service.Languages.FirstOrDefault(c => c.Code == _cultre);
            var model = new Detail
            {
                Id = service.Id,
                CategoryName = service.Category.Languages.FirstOrDefault(c => c.Code == _cultre).Title,
                Description = lang.Description,
                Href = lang.Href,
                ImageUrl = service.ImageUrl,
                Title = lang.Title
            };

            return model;
        }
    }
}