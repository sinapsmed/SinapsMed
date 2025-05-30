using System.Linq.Expressions;
using Entities.Concrete.OrderEntities;
using Entities.DTOs.AddountantDtos.Body;
using Entities.DTOs.AddountantDtos.ReturnData;

namespace DataAccess.Concrete.SQLServer.EFDALs.Accountatnts
{
    public static class AccountantSelector
    {
        public static Expression<Func<Payment, PaymentDto>> Payment()
        {
            return payment => new PaymentDto
            {
                Id = payment.Id,
                CreatedAt = payment.CreatedAt,
                Amount = payment.Amount,
                UnikalKey = payment.UnikalKey,
            };
        }

        public static DetailedPaymentDto DetailedPayment(Payment payment)
        {
            return new DetailedPaymentDto
            {
                Amount = payment.Amount,
                CreatedAt = payment.CreatedAt,
                Cupon = payment.Cupon,
                Currency = payment.Currency,
                Description = payment.Description,
                Id = payment.Id,
                Language = payment.Language,
                Status = payment.Status,
                UnikalKey = payment.UnikalKey,
                UserUnikalKey = payment.User.UnicalKey
            };
        }

        internal static Expression<Func<OrderItem, ClinicDto>> ClinicSalesRecord()
        {
            return c => new ClinicDto
            {
                Email = c.Analysis.Clinic.Email.Email,
                Name = c.Analysis.Clinic.Name,
                AnalysesPrice = c.Amount,
                AnalysesPriceDiscounted = c.Amount - (c.Amount * c.Count * c.Analysis.Category.DiscountedPercent / 100),
                UsedAnalyses = c.IsUsed ? c.Count : 0,
                AnalysesFee = c.Amount * c.Count * c.Analysis.Category.DiscountedPercent / 100
            };
        }

        internal static Expression<Func<OrderItem, AppointmentDtoData>> AppointmentSalesRecord()
        {
            throw new NotImplementedException();
        }
    }
}