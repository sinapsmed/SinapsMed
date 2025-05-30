using System.Linq.Expressions;
using DataAccess.Services;
using Entities.Concrete.Appointments;
using Entities.Concrete.Clinics;
using Entities.Concrete.OrderEntities;
using Entities.Concrete.Staff;
using Entities.Concrete.UserEntities;
using Entities.DTOs.AnalysisDtos.Analysis;
using Entities.DTOs.AppointmentsDtos.GetData;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.DTOs.ExpertDtos.GetDtos;
using Entities.DTOs.ServiceDtos.Get;
using Entities.DTOs.StaffDtos.Return;
using Entities.DTOs.Users;

namespace DataAccess.Concrete.SQLServer.EFDALs.Staff
{
    public static class StaffSelector
    {
        public static Expression<Func<Support, AllStaff>> All()
        {
            return staff => new AllStaff
            {
                Email = staff.WorkSpaceEmail.Email,
                FullName = staff.FullName,
                Id = staff.Id
            };
        }

        public static Expression<Func<OrderItem, StaffAnalyses>> Analyses()
        {
            return c => new StaffAnalyses
            {
                User = new DetailedUser
                {
                    DateOfBrith = c.Order.User.DateOfBrith,
                    Email = c.Order.User.Email,
                    FullName = c.Order.User.FullName,
                    Gender = c.Order.User.Gender,
                    Id = c.Order.User.Id,
                    ImageUrl = c.Order.User.ImageUrl,
                    UnikalId = c.Order.User.UnicalKey,
                },
                Clinic = new ClinicBasketDetail
                {
                    Id = c.Clinic.Id,
                    Name = c.Clinic.Name,
                    UnicalKey = c.Clinic.UnicalKey,
                    MapLink = EFService.MapUrl(c.Clinic.Latitude, c.Clinic.Longitude)
                },
                Code = c.Analysis.Code,
                Discounted = double.Round(c.Analysis.Price - ((double)c.Analysis.Price * c.Analysis.Category.DiscountedPercent / 100), 2),
                Id = c.Analysis.Id,
                Name = c.Analysis.Name,
                Price = c.Analysis.Price
            };
        }
    }
}