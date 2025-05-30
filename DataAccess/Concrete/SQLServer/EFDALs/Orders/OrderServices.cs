using System.Text;
using System.Threading.Tasks;
using Core.Helpers.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.BasketDtos.BodyDtos;
using Entities.Enums;
namespace DataAccess.Concrete.SQLServer.EFDALs.Orders
{
    public static class OrderServices
    {
        private static readonly Random _random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        public static string GenerateUniqueUnicalKey()
        {
            string newId;

            newId = "SMO-" + new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[_random.Next(s.Length)]).ToArray());

            return newId;
        }

        public static string GenerateUniqueUnicalKeyForPayment(AppDbContext dbContext)
        {
            string newId;
            do
            {
                newId = "SMP-" + new string(Enumerable.Repeat(chars, 5)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            while (dbContext.Payments.Any(u => u.UnikalKey == newId));

            return newId;
        }

        public static Guid? GetCorrectId(MyBasketItem item, ItemType type)
        {
            if (item.Type == type)
            {
                return item.ServiceId;
            }
            else
            {
                return null;
            }
        }


    }
}