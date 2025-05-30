using Core.DataAccess;
using Entities.Concrete.Locations;
using Microsoft.Extensions.Localization;
using Entities.DTOs.LocationDtos.Common;
using Entities.Concrete.Clinics;
using Entities.DTOs.LocationDtos.Clinics;
using DataAccess.Concrete.SQLServer.DataBase;

namespace DataAccess.Concrete.SQLServer.EFDALs.Locations.CRUD
{
    public class EFLocationUpdateDAL : LocationsAdapter
    {
        public EFLocationUpdateDAL(
            AppDbContext context,
            IStringLocalizer<LocationsAdapter> localizer,
            IRepositoryBase<City, Get, AppDbContext> cityRepo,
            IRepositoryBase<Region, Get, AppDbContext> regionRepo,
            IRepositoryBase<Village, Get, AppDbContext> villageRepo,
             IRepositoryBase<Clinic, GetClinics, AppDbContext> clinicRepo
            ) : base(context, localizer, cityRepo, regionRepo, villageRepo, clinicRepo)
        {

        }
    }
}