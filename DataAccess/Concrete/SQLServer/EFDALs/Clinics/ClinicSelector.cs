using System.Linq.Expressions;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.ClinicDtos.DataDtos;

namespace DataAccess.Concrete.SQLServer.EFDALs.Clinics
{
    public static class ClinicSelector
    {
        public static OrderItemDetail ItemDetail(List<OrderItem> item)
        {
            return new OrderItemDetail
            {
                AnalysisDetail = item.Select(item => new ClinicAnalysisDetail
                {
                    Category = item.Analysis.Category.Name,
                    Code = item.Analysis.Code,
                    Id = item.Id,
                    Name = item.Analysis.Name,
                    Count = item.Count,
                    IsUsed = item.IsUsed,
                    ItemUnikalKey = item.UnikalKey,
                    User = new OrderItemUser
                    {
                        FullName = item.Order.User.FullName,
                        Gender = item.Order.User.Gender,
                        ImageUrl = item.Order.User.ImageUrl,
                        UnicalKey = item.Order.User.UnicalKey
                    }
                }).ToList(),
            };
        }

        public static Expression<Func<OrderItem, ClinicOrderItem>> OrderItems()
        {
            return c => new ClinicOrderItem
            {
                Code = c.Analysis != null ? c.Analysis.Code : "",
                Count = c.Count,
                Id = c.Id,
                IsUsed = c.IsUsed,
                UserFullName = c.Order.User.FullName,
                UnikalKey = c.Order.User.ImageUrl,
                UserImage = c.Order.User.UnicalKey,
                AnalysisName = c.Analysis.Name,
                ItemUnikalKey = c.UnikalKey
            };
        }
    }
}