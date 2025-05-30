using Microsoft.Extensions.Localization;
using System.Net;
using Core.DataAccess;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.Concrete.Clinics;
using Entities.Concrete.Locations;
using Entities.DTOs.LocationDtos.Clinics;
using Entities.DTOs.LocationDtos.Common;
using Microsoft.EntityFrameworkCore;
using DataAccess.Concrete.SQLServer.DataBase;
namespace DataAccess.Concrete.SQLServer.EFDALs.Locations.CRUD
{
    public class EFLocationDeleteDAL : LocationsAdapter
    {
        public EFLocationDeleteDAL(
            AppDbContext context,
            IStringLocalizer<LocationsAdapter> localizer,
            IRepositoryBase<City, Get, AppDbContext> cityRepo,
            IRepositoryBase<Region, Get, AppDbContext> regionRepo,
            IRepositoryBase<Village, Get, AppDbContext> villageRepo,
             IRepositoryBase<Clinic, GetClinics, AppDbContext> clinicRepo
            ) : base(context, localizer, cityRepo, regionRepo, villageRepo, clinicRepo)
        {

        }

        public override async Task<IResult> DeleteClinic(Guid id)
        {
            Clinic clinic = await _context.Clinics.FirstOrDefaultAsync(c => c.Id == id);
            if (clinic is null)
                return new ErrorResult(_dalLoclizer["notFoundCl"], HttpStatusCode.NotFound, $"We Cant find Clinic with id {id}");
            return await _clinicRepo.Remove(clinic, _context);
        }

        public override async Task<IResult> DeleteCity(Guid id)
        {
            City city = await _context.Cities.FirstOrDefaultAsync(c => c.Id == id);
            if (city is null)
                return new ErrorResult(_dalLoclizer["notFoundC"], HttpStatusCode.NotFound, $"We Cant find city with id {id}");
            return await _cityRepo.Remove(city, _context);
        }

        public override async Task<IResult> DeleteRegion(Guid id)
        {
            Region region = await _context.Regions.Include(c => c.Villages).FirstOrDefaultAsync(c => c.Id == id);
            if (region is null)
                return new ErrorResult(_dalLoclizer["notFoundR"], HttpStatusCode.NotFound, $"We Cant find region with id {id}");

            if (region.Villages.Count() > 0)
                return new ErrorResult(_dalLoclizer["regionHasVillages"]);
            return await _regionRepo.Remove(region, _context);
        }

        public override async Task<IResult> DeleteVillage(Guid id)
        {
            Village village = await _context.Villages.Include(c => c.Clinics).FirstOrDefaultAsync(c => c.Id == id);

            if (village is null)
                return new ErrorResult(_dalLoclizer["notFoundV"], HttpStatusCode.NotFound, $"We Cant find village with id {id}");

            if (village.Clinics.Count() > 0)
                return new ErrorResult(_dalLoclizer["villageHasClinics"]);

            return await _villageRepo.Remove(village, _context);
        }

    }
}