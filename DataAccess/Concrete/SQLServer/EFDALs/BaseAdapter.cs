using System.Globalization;
using DataAccess.Concrete.SQLServer.DataBase;

namespace DataAccess.Concrete.SQLServer.EFDALs
{
    public class BaseAdapter    
    {
        protected readonly AppDbContext _context;
        protected readonly string _cultre;

        public BaseAdapter(AppDbContext context)
        {
            _context = context;
            _cultre = CultureInfo.CurrentCulture.Name;
        }
    }
}