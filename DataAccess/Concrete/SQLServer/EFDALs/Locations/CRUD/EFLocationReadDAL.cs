using Microsoft.Extensions.Localization;
using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.Concrete.Clinics;
using Entities.Concrete.Locations;
using Entities.DTOs.LocationDtos.Clinics;
using Entities.DTOs.LocationDtos.Common;
using Core.Helpers;
using System.Globalization;
using System.Net;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Microsoft.EntityFrameworkCore;
using DataAccess.Services.Abstract;
using Entities.DTOs.LocationDtos.Admin;
using DataAccess.Concrete.SQLServer.DataBase;
using Google.Apis.Util;

namespace DataAccess.Concrete.SQLServer.EFDALs.Locations.CRUD
{
    public class EFLocationReadDAL : LocationsAdapter
    {
        private readonly IDAtaAccessService _service;
        private readonly IRepositoryBase<Village, VillageDetailed, AppDbContext> _villageDetailed;
        private readonly IRepositoryBase<Clinic, ClinicsDetailed, AppDbContext> _clinicDetailed;
        public EFLocationReadDAL(
            AppDbContext context,
            IStringLocalizer<LocationsAdapter> localizer,
            IRepositoryBase<City, Get, AppDbContext> cityRepo,
            IRepositoryBase<Region, Get, AppDbContext> regionRepo,
            IRepositoryBase<Village, Get, AppDbContext> villageRepo,
            IRepositoryBase<Clinic, GetClinics, AppDbContext> clinicRepo,
            IDAtaAccessService service,
            IRepositoryBase<Village, VillageDetailed, AppDbContext> villageDetailed,
            IRepositoryBase<Clinic, ClinicsDetailed, AppDbContext> clinicDetailed) : base(context, localizer, cityRepo, regionRepo, villageRepo, clinicRepo)
        {
            _service = service;
            _villageDetailed = villageDetailed;
            _clinicDetailed = clinicDetailed;
        }

        public override async Task<IDataResult<BaseDto<ClinicsDetailed>>> Clinics(Guid? villageId, int page, int limit)
        {
            IQueryable<Clinic> clinics = _context.Set<Clinic>()
                .Include(c => c.Village)
                    .ThenInclude(c => c.Region)
                        .ThenInclude(c => c.City)
                .Include(c => c.Email);

            if (villageId is not null)
            {
                clinics = clinics.Where(c => c.VillageId == villageId);
            }
            var filter = new DtoFilter<Clinic, ClinicsDetailed>
            {
                Limit = limit,
                Page = page,
                Selector = LocationSelector.ClinicsDetailed()
            };
            return await _clinicDetailed.GetAllAsync(clinics, filter);
        }

        public override async Task<IDataResult<BaseDto<Get>>> Cities(int page, int limit)
        {
            var cities = _context.Set<City>().OrderBy(c => c.Name);

            var filter = new DtoFilter<City, Get>
            {
                Limit = limit,
                Page = page,
                Selector = LocationSelector.ReturnCities()
            };

            return await _cityRepo.GetAllAsync(cities, filter);
        }

        public async override Task<IDataResult<BaseDto<VillageDetailed>>> Villages(Guid? regionId, int page, int limit)
        {
            IQueryable<Village> villages = _context.Set<Village>()
                .OrderBy(c => c.Name)
                .Include(c => c.Partners);

            if (regionId is not null)
            {
                villages = villages.Where(c => c.RegionId == regionId);
            }

            var filter = new DtoFilter<Village, VillageDetailed>
            {
                Limit = limit,
                Page = page,
                Selector = LocationSelector.VillagesDetailed()
            };

            return await _villageDetailed.GetAllAsync(villages, filter);
        }

        public override async Task<IDataResult<BaseDto<GetClinics>>> Clinics(string? location, Guid? cityId, Guid? regionId, Guid? villageId, Guid? partnerId, int page, int limit)
        {
            IQueryable<Clinic> clinics = _context.Set<Clinic>();

            if (partnerId is not null)
            {
                clinics = clinics.Where(c => c.Village.Partners.Any(p => p.Id == partnerId));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                BaseDto<GetClinics> data = new BaseDto<GetClinics>();
                var pageCount = Math.Ceiling((double)clinics.Count() / limit);

                string[] locs = location.Split(", ");
                if (locs.Length != 2 ||
                    !double.TryParse(locs[0], NumberStyles.Any, CultureInfo.InvariantCulture, out double latitude) ||
                    !double.TryParse(locs[1], NumberStyles.Any, CultureInfo.InvariantCulture, out double longitude))
                {
                    return new ErrorDataResult<BaseDto<GetClinics>>(_dalLoclizer["locationParse"], HttpStatusCode.BadRequest, $"Wrong Location string {location}");
                }

                var sortedClinics = clinics
                    .ToList()
                    .Select(c => new
                    {
                        Clinic = c,
                        Distance = _service.Distance(c.Latitude, c.Longitude, latitude, longitude)
                    })
                    .OrderBy(cd => cd.Distance)
                    .Skip((page - 1) * limit)
                    .Take(limit)
                    .Select(cd => new GetClinics
                    {
                        Id = cd.Clinic.Id,
                        Name = cd.Clinic.Name,
                        Distance = cd.Distance
                    })
                    .ToList();

                data = new BaseDto<GetClinics>
                {
                    CurrentPage = page,
                    Data = sortedClinics,
                    PageCount = (int)pageCount
                };

                return new SuccessDataResult<BaseDto<GetClinics>>(data, HttpStatusCode.OK);
            }
            else if (villageId is not null)
            {
                clinics = clinics.Where(c => c.VillageId == villageId);

                var filter = new DtoFilter<Clinic, GetClinics>
                {
                    Limit = limit,
                    Page = page,
                    Selector = cd => new GetClinics
                    {
                        Id = cd.Id,
                        Name = cd.Name,
                        Distance = 0
                    }
                };
                return await _clinicRepo.GetAllAsync(clinics, filter);
            }
            else if (regionId is not null)
            {
                clinics = clinics.Include(c => c.Village).Where(c => c.Village.RegionId == regionId);

                var filter = new DtoFilter<Clinic, GetClinics>
                {
                    Limit = limit,
                    Page = page,
                    Selector = cd => new GetClinics
                    {
                        Id = cd.Id,
                        Name = cd.Name,
                        Distance = 0
                    }
                };
                return await _clinicRepo.GetAllAsync(clinics, filter);
            }
            else if (cityId is not null)
            {
                clinics = clinics.Include(c => c.Village).ThenInclude(c => c.Region).Where(c => c.Village.Region.CityId == cityId);

                var filter = new DtoFilter<Clinic, GetClinics>
                {
                    Limit = limit,
                    Page = page,
                    Selector = cd => new GetClinics
                    {
                        Id = cd.Id,
                        Name = cd.Name,
                        Distance = 0
                    }
                };
                return await _clinicRepo.GetAllAsync(clinics, filter);
            }
            else
            {
                var filter = new DtoFilter<Clinic, GetClinics>
                {
                    Limit = limit,
                    Page = page,
                    Selector = cd => new GetClinics
                    {
                        Id = cd.Id,
                        Name = cd.Name,
                        Distance = 0
                    }
                };
                return await _clinicRepo.GetAllAsync(clinics, filter);
            }
        }

        public override async Task<IDataResult<BaseDto<Get>>> Regions(Guid id, int limit, int page)
        {
            var regions = _context.Set<Region>()
                .OrderBy(c => c.Name)
                .Where(c => c.CityId == id);

            var filter = new DtoFilter<Region, Get>
            {
                Limit = limit,
                Page = page,
                Selector = LocationSelector.ReturnRegions()
            };

            return await _regionRepo.GetAllAsync(regions, filter);
        }

        public override async Task<IDataResult<BaseDto<Get>>> Villages(Guid cityId, Guid? regionId, int page, int limit)
        {
            IQueryable<Village> villages;

            if (regionId is not null)
            {
                villages = _context.Set<Village>().Where(c => c.RegionId == regionId);
            }
            else
            {
                var data = await _context.Villages.Include(c => c.Region).Where(c => c.Region.CityId == cityId).ToListAsync();
                villages = _context.Set<Village>().Include(c => c.Region).Where(c => c.Region.CityId == cityId);
            }

            var filter = new DtoFilter<Village, Get>
            {
                Limit = limit,
                Page = page,
                Selector = LocationSelector.ReturnVillages()
            };

            return await _villageRepo.GetAllAsync(villages, filter);
        }

    }
}