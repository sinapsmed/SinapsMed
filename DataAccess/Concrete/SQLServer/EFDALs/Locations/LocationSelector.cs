using System.Linq.Expressions;
using Entities.Concrete.Clinics;
using Entities.Concrete.Locations;
using Entities.DTOs.LocationDtos.Admin;
using Entities.DTOs.LocationDtos.Common;

namespace DataAccess.Concrete.SQLServer.EFDALs.Locations
{
    public class LocationSelector
    {
        public static Expression<Func<Clinic, ClinicsDetailed>> ClinicsDetailed()
        {
            return c => new ClinicsDetailed
            {
                Id = c.Id,
                Name = c.Name,
                Latitude = c.Latitude,
                Longitude = c.Longitude,
                Location = $"{c.Village.Region.City.Name}, {c.Village.Region.Name}, {c.Village.Name}",
                UnikalKey = c.UnicalKey,
                Email = c.Email.Email
            };
        }
        public static Expression<Func<City, Get>> ReturnCities()
        {
            return c => new Get
            {
                Id = c.Id,
                Name = c.Name,
            };
        }

        public static Expression<Func<Region, Get>> ReturnRegions()
        {
            return c => new Get
            {
                Id = c.Id,
                Name = c.Name,
            };
        }

        public static Expression<Func<Village, Get>> ReturnVillages()
        {
            return c => new Get
            {
                Id = c.Id,
                Name = c.Name,
            };
        }

        public static Expression<Func<Village, VillageDetailed>> VillagesDetailed()
        {
            return c => new VillageDetailed
            {
                Id = c.Id,
                Name = c.Name,
                Partners = c.Partners.Select(c => c.Name)
            };
        }
    }
}