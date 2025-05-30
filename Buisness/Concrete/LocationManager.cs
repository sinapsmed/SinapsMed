using System.Net;
using Buisness.Abstract;
using Buisness.Infrastructure.Factories.Locations;
using Buisness.Services.Static;
using Buisness.Validation.Location;
using Core.Entities;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using DataAccess.Concrete.SQLServer.DataBase;
using Entities.DTOs.LocationDtos;
using Entities.DTOs.LocationDtos.Admin;
using Entities.DTOs.LocationDtos.Clinics;
using Entities.DTOs.LocationDtos.Common;
using Entities.Enums;
using Microsoft.Extensions.Localization;

namespace Buisness.Concrete
{
    public class LocationManager : Manager, ILocationService
    {
        private readonly IStringLocalizer<Validator> _common;
        private readonly AppDbContext _context;
        private readonly LocationServiceFactory _factory;

        public LocationManager(IStringLocalizer<Validator> common, AppDbContext context, LocationServiceFactory factory)
        {
            _common = common;
            _context = context;
            _factory = factory;
        }

        public async Task<IResult> AddCity(CreateCity createCity)
        {
            try
            {
                var result = await GenericValidator<CreateCity, CityCreateValidator>.ValidateAsync(createCity, new CityCreateValidator(_context, _common));
                if (!result.Success)
                    return result;
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddCity(createCity);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddClinic(CreateClinic createClinic)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddClinic(createClinic);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddRegion(CreateRegion createRegion)
        {
            try
            {
                var result = await GenericValidator<CreateRegion, RegionCreateValidator>.ValidateAsync(createRegion, new RegionCreateValidator(_context, _common));
                if (!result.Success)
                    return result;
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddRegion(createRegion);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> AddVillage(CreateVillage createVillage)
        {
            try
            {
                var result = await GenericValidator<CreateVillage, VillageCreateValidator>.ValidateAsync(createVillage, new VillageCreateValidator(_context, _common));
                if (!result.Success)
                    return result;
                var _dal = _factory.GetService(ServiceFactoryType.Create);
                var response = await _dal.AddVillage(createVillage);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Get>>> Cities(int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.Cities(page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Get>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<GetClinics>>> Clinics(string? location, Guid? cityId, Guid? regionId, Guid? villageId, Guid? partnerId, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);

                var response = await _dal.Clinics(location, cityId, regionId, villageId, partnerId, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<GetClinics>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<ClinicsDetailed>>> Clinics(Guid? villageId, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);

                var response = await _dal.Clinics(villageId, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<ClinicsDetailed>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteCity(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);

                var response = await _dal.DeleteCity(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteClinic(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);

                var response = await _dal.DeleteClinic(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteRegion(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);

                var response = await _dal.DeleteRegion(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IResult> DeleteVillage(Guid id)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Delete);
                var response = await _dal.DeleteVillage(id);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorResult(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Get>>> Regions(Guid id, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.Regions(id, limit, page);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Get>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<Get>>> Villages(Guid cityId, Guid? regionId, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.Villages(cityId, regionId, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<Get>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }

        public async Task<IDataResult<BaseDto<VillageDetailed>>> Villages(Guid? regionId, int page, int limit)
        {
            try
            {
                var _dal = _factory.GetService(ServiceFactoryType.Read);
                var response = await _dal.Villages(regionId, page, limit);
                return response;
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<BaseDto<VillageDetailed>>(_common["ex"], HttpStatusCode.BadRequest, ex);
            }
        }
    }
}