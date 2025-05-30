using DataAccess.Services;
using Entities.Concrete.BasketEntities;
using Entities.DTOs.BasketDtos.BodyDtos;
using Entities.DTOs.ClinicDtos.DataDtos;
using Entities.Enums;

namespace DataAccess.Concrete.SQLServer.EFDALs.Baskets
{
    public static class BasketSelector
    {
        public static Func<BasketItem, MyBasketItem> Items(string cultre, IEnumerable<Guid> serviceIds, CuponType cuponType = CuponType.Common, byte discount = 0)
        {
            return c => new MyBasketItem
            {
                Amount = GetPrice(c),
                Count = c.Count,
                Title = ItemTitle(c, cultre),
                Discounted = ItemDiscounted(c, GetPrice(c), CorrectId(c), cuponType, discount, serviceIds),
                Id = c.Id,
                Type = c.Type,
                ServiceId = GetServiceId(c),
                ClinicDetail = GetBasketDetail(c)
            };
        }
        private static ClinicBasketDetail? GetBasketDetail(BasketItem item)
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

        private static Guid GetServiceId(BasketItem c)
        {
            if (c.Type is ItemType.Analysis)
            {
                return Guid.Parse(c.AnalysisId.ToString());
            }
            else if (c.Type is ItemType.Appointment)
            {
                return Guid.Parse(c.AppointmentId.ToString());
            }
            else
            {
                return default;
            }
        }

        public static float GetPrice(BasketItem item)
        {
            if (item.Type is ItemType.Analysis)
            {
                return (float)item.Analysis.Price;
            }
            else if (item.Type is ItemType.Appointment)
            {
                return (float)item.Appointment.ServicePeriod.ExpertPeriods.FirstOrDefault(c => c.ExpertId
                == item.Appointment.ExpertId).Price;
            }
            else
            {
                return 0;
            }
        }

        public static Guid CorrectId(BasketItem item)
        {
            if (item.AnalysisId == default(Guid) || item.AnalysisId == null)
            {
                return Guid.Parse(item.AppointmentId.ToString());
            }
            return Guid.Parse(item.AnalysisId.ToString());
        }

        public static float ItemDiscounted(BasketItem item, float price, Guid serviceId, CuponType cuponType, byte discount, IEnumerable<Guid> serviceIds)
        {
            if (item.Type is ItemType.Analysis)
            {
                double categoryDiscount = item.Analysis.Category.DiscountedPercent;
                price -= (float)(price * categoryDiscount) / 100;
            }

            if ((serviceIds.Contains(serviceId) || serviceIds.Count() == 0) && ((int)item.Type == (int)cuponType || cuponType == CuponType.Common))
            {
                price = price - ((float)(price * discount) / 100);
            }

            return (float)Math.Round(price, 2);
        }

        public static string ItemTitle(BasketItem item, string cultre)
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