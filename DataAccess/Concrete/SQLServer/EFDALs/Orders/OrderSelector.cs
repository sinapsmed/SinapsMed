using System.Linq.Expressions;
using DataAccess.Services;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.DTOs.OrderDtos.BodyDtos;
using Entities.Enums;

namespace DataAccess.Concrete.SQLServer.EFDALs.Orders
{
    public static class OrderSelector
    {
        public static Expression<Func<Order, GetAll>> GetAll(string cultre)
        {
            return c => new GetAll
            {
                CreatedAt = c.CreatedAt,
                Id = c.Id,
                Number = c.Number,
                ItemCount = c.Items.Count(),
                TotalAmount = c.Items.Sum(x => x.Amount),
            };
        }

        public static ClinicBasketDetail? GetClinicDetail(OrderItem item)
        {
            if (item.Type is not ItemType.Analysis)
            {
                return null;
            }

            return new ClinicBasketDetail
            {
                Id = Guid.Parse(item.ClinicId.ToString()),
                Name = item.Clinic.Name,
                UnicalKey = item.Clinic.UnicalKey,
                MapLink = EFService.MapUrl(item.Clinic.Latitude, item.Clinic.Longitude)
            };
        }

        public static string ItemTitle(OrderItem item, string cultre)
        {
            if (item.Type is ItemType.Analysis)
            {
                return $"{item.Analysis.Name} ({item.Analysis.Code})";
            }
            else if (item.Type is ItemType.Appointment)
            {
                var service = item.Appointment.ServicePeriod.Service.Languages.FirstOrDefault(c => c.Code == cultre);
                return $"{service.Title}";
            }
            else
            {
                return "Something was wrong...";
            }
        }
    }
}