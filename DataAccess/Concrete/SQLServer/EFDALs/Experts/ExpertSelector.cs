using System.Linq.Expressions;
using Core.Helpers;
using Entities.Concrete.Appointments;
using Entities.Concrete.Experts;
using Entities.Concrete.Services;
using Entities.DTOs.AppointmentsDtos.Body;
using Entities.DTOs.ExpertDtos;
using Entities.DTOs.ExpertDtos.AdminDtos.Get;
using Entities.DTOs.ExpertDtos.GetDtos;

namespace DataAccess.Concrete.SQLServer.EFDALs.Experts
{
    public class ExpertSelector
    {

        public static Expression<Func<Appointment, ExpertAppointment>> ExpertAppointments()
        {
            return c => new ExpertAppointment
            {
                Date = c.Date,
                Id = c.Id,
                Price = c.Price,
                UnikalKey = c.UnicalKey,
                UserFullName = c.User.FullName,
                UserPhotoUrl = c.User.ImageUrl
            };
        }
        public static Expression<Func<ServiceCategory, SpecCategories>> SpecCategories(string cultre)
        {
            return c => new SpecCategories
            {
                Id = c.Id,
                Title = c.Languages.FirstOrDefault(x => x.Code == cultre).Title
            };
        }

        public static Expression<Func<Service, SpecalityGetDto>> Specs(string cultre)
        {
            return c => new SpecalityGetDto
            {
                Id = c.Id,
                Title = c.Languages.FirstOrDefault(x => x.Code == cultre).Title
            };
        }

        public static Expression<Func<Expert, ExpertDetailedList>> ExpertDetailedList(string cultre)
        {
            return c => new ExpertDetailedList
            {
                FullName = c.FullName,
                Id = c.Id,
                PhotoUrl = c.PhotoUrl,
                Resume = c.Resume,
                Services = string.Join(", ", c.Services
                        .Select(x => x.Languages.FirstOrDefault(l => l.Code == cultre).Title)),
                Email = c.Email,
                Fee = c.Fee,
                Specality = c.Specality,
            };
        }

        public static Expression<Func<Expert, ExpertList>> ExpertList(string cultre)
        {
            return c => new ExpertList
            {
                FullName = c.FullName,
                Id = c.Id,
                PhotoUrl = c.PhotoUrl,
                Resume = c.Resume,
                Services = string.Join(", ", c.Services
                        .Select(x => x.Languages.FirstOrDefault(l => l.Code == cultre).Title)),
                Seo = SeoHelper.ConverToSeo(c.FullName, cultre),
                Specality = c.Specality,
            };
        }

        public static Expression<Func<Expert, GetBoosted>> BoostedExpert(string cultre)
        {
            return c => new GetBoosted
            {
                FullName = string.Join(' ', c.Specality, c.FullName),
                Id = c.Id,
                PhotoUrl = c.PhotoUrl,
                SeoUrl = c.FullName.ConverToSeo(cultre),
                Specialties = new List<string> { "Psixatr" }
            };
        }
    }
}