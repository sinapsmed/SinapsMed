using DataAccess.Concrete.SQLServer.DataBase;

namespace DataAccess.Concrete.SQLServer.EFDALs.Auth
{
    public static class AuthService
    {
        private static readonly Random _random = new Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        public static string GenerateUniqueUnicalKey(AppDbContext dbContext)
        {
            string newId;
            do
            {
                newId = "SM-" + new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[_random.Next(s.Length)]).ToArray());
            }
            while (dbContext.Users.Any(u => u.UnicalKey == newId));

            return newId;
        } 
    }
}