using Core.DataAccess;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.SQLServer.DataBase;
using DataAccess.Exceptions.Global;
using Entities.Concrete.Clinics;
using Entities.Concrete.Locations;
using Entities.DTOs.LocationDtos;
using Entities.DTOs.LocationDtos.Admin;
using Entities.DTOs.LocationDtos.Clinics;
using Entities.DTOs.LocationDtos.Common;
using Microsoft.Extensions.Localization;

namespace DataAccess.Concrete.SQLServer.EFDALs.Locations
{
    public class LocationsAdapter : BaseAdapter, ILocationDAL
    {
        protected readonly IStringLocalizer<LocationsAdapter> _dalLoclizer;
        protected readonly IRepositoryBase<City, Get, AppDbContext> _cityRepo;
        protected readonly IRepositoryBase<Region, Get, AppDbContext> _regionRepo;
        protected readonly IRepositoryBase<Village, Get, AppDbContext> _villageRepo;
        protected readonly IRepositoryBase<Clinic, GetClinics, AppDbContext> _clinicRepo;


        public LocationsAdapter(
            AppDbContext context,
            IStringLocalizer<LocationsAdapter> dalLoclizer,
            IRepositoryBase<City, Get, AppDbContext> cityRepo,
            IRepositoryBase<Region, Get, AppDbContext> regionRepo,
            IRepositoryBase<Village, Get, AppDbContext> villageRepo,
            IRepositoryBase<Clinic, GetClinics, AppDbContext> clinicRepo) : base(context)
        {
            _dalLoclizer = dalLoclizer;
            _cityRepo = cityRepo;
            _regionRepo = regionRepo;
            _villageRepo = villageRepo;
            _clinicRepo = clinicRepo;
        }

        public virtual Task<IResult> AddCity(CreateCity createCity)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> AddClinic(CreateClinic createClinic)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> AddRegion(CreateRegion createRegion)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> AddVillage(CreateVillage createVillage)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<Get>>> Cities(int page, int limit)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<GetClinics>>> Clinics(string? location, Guid? cityId, Guid? regionId, Guid? villageId, Guid? partnerId, int page, int limit)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<ClinicsDetailed>>> Clinics(Guid? villageId, int page, int limit)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> DeleteCity(Guid id)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> DeleteClinic(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IResult> DeleteRegion(Guid id)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IResult> DeleteVillage(Guid id)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<Get>>> Regions(Guid id, int limit, int page)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<Get>>> Villages(Guid cityId, Guid? regionId, int page, int limit)
        {
            throw new SystemNotWorkingException();
        }

        public virtual Task<IDataResult<BaseDto<VillageDetailed>>> Villages(Guid? regionId, int page, int limit)
        {
            throw new SystemNotWorkingException();
        }
    }
}