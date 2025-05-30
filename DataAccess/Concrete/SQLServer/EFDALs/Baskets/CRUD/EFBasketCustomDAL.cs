using DataAccess.Concrete.SQLServer.DataBase;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Baskets.CRUD
{
    public class EFBasketCustomDAL : BasketAdapter
    {
        public EFBasketCustomDAL(
            AppDbContext context,
            IStringLocalizer<BasketAdapter> dalLocalizer) : base(context, dalLocalizer)
        {
        }
    }
}