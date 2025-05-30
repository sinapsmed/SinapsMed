using Core.Utilities.Results.Abstract;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Net;
using Core.DataAccess;
using Core.Utilities.Results.Concrete.ErrorResult;
using Entities.Concrete.Clinics;
using Entities.Concrete.Locations;
using Entities.DTOs.LocationDtos;
using Entities.DTOs.LocationDtos.Clinics;
using Entities.DTOs.LocationDtos.Common;
using Microsoft.EntityFrameworkCore;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Services.Abstract;
using Microsoft.Extensions.Configuration;
using DataAccess.Concrete.SQLServer.EFDALs.Emails.CRUD;
namespace DataAccess.Concrete.SQLServer.EFDALs.Locations.CRUD
{
    public class EFLocationCreateDAL : LocationsAdapter
    {
        private readonly IDAtaAccessService _dataAccess;
        private readonly EFWorkSpaceEmailCreateDAL _emailDAL;
        private readonly IConfiguration _config;
        public EFLocationCreateDAL(
            AppDbContext context,
            IStringLocalizer<LocationsAdapter> localizer,
            IRepositoryBase<City, Get, AppDbContext> cityRepo,
            IRepositoryBase<Region, Get, AppDbContext> regionRepo,
            IRepositoryBase<Village, Get, AppDbContext> villageRepo,
             IRepositoryBase<Clinic, GetClinics, AppDbContext> clinicRepo,
             IDAtaAccessService dataAccess,
             IConfiguration config,
             EFWorkSpaceEmailCreateDAL emailDAL) : base(context, localizer, cityRepo, regionRepo, villageRepo, clinicRepo)
        {
            _dataAccess = dataAccess;
            _config = config;
            _emailDAL = emailDAL;
        }

        public override async Task<IResult> AddCity(CreateCity createCity)
        {
            var partners = await _context.Partners
                .ToListAsync();

            City city = new City
            {
                Name = createCity.Name,
            };

            return await _cityRepo.AddAsync(city, _context);
        }

        public override async Task<IResult> AddClinic(CreateClinic createClinic)
        {
            string[] locs = createClinic.Location.Split(", ");

            if (locs.Length != 2 ||
                !double.TryParse(locs[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude) ||
                !double.TryParse(locs[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude))
            {
                return new ErrorResult(_dalLoclizer["locationParse"], HttpStatusCode.BadRequest, $"Wrong Location string {createClinic.Location}");
            }

            Clinic clinic = new Clinic
            {
                Latitude = latitude,
                Longitude = longitude,
                Name = createClinic.Name,
                VillageId = createClinic.VillageId,
                UnicalKey = LocationServices.GenerateUniqueUnicalKey(_context),
            };

            string email = $"{clinic.UnicalKey.ToLower()}@{_config["Email:Domain"]}";

            var result = await _emailDAL.AddAsync(email, createClinic.Password, createClinic.Name);

            if (!result.Success)
                return result;

            var workSpaceEmail = await _context.Emails
                .FirstOrDefaultAsync(c => c.Email == email);

            if (workSpaceEmail == null)
                return new ErrorResult(_dalLoclizer["emailNotFound"], HttpStatusCode.BadRequest, $"Email not found {email}");

            clinic.EmailId = workSpaceEmail.Id;

            return await _clinicRepo.AddAsync(clinic, _context);
        }

        public override async Task<IResult> AddRegion(CreateRegion createRegion)
        {
            var partners = await _context.Partners
                .ToListAsync();
            Region region = new Region
            {
                CityId = createRegion.CityId,
                Name = createRegion.Name,
            };
            return await _regionRepo.AddAsync(region, _context);
        }

        public override async Task<IResult> AddVillage(CreateVillage createVillage)
        {

            var partners = await _context.Partners
                .Where(c => createVillage.Partners.Any(x => x == c.Id))
                .ToListAsync();

            Village village = new Village
            {
                Name = createVillage.Name,
                Partners = partners,
                RegionId = createVillage.RegionId
            };
            return await _villageRepo.AddAsync(village, _context);
        }

    }
}