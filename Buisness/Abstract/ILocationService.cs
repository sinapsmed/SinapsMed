using Core.Entities;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.LocationDtos;
using Entities.DTOs.LocationDtos.Admin;
using Entities.DTOs.LocationDtos.Clinics;
using Entities.DTOs.LocationDtos.Common;

namespace Buisness.Abstract
{
    public interface ILocationService : IService
    {
        Task<IResult> AddCity(CreateCity createCity); // can create this city Resgins and villages here 
        Task<IResult> AddRegion(CreateRegion createRegion); //with City Id for add this city
        Task<IResult> AddVillage(CreateVillage createVillage); //with Region Id for add this Region
        Task<IResult> AddClinic(CreateClinic createClinic);

        Task<IDataResult<BaseDto<Get>>> Cities(int page, int limit);
        Task<IDataResult<BaseDto<Get>>> Regions(Guid id, int page, int limit);//With City Id
        Task<IDataResult<BaseDto<VillageDetailed>>> Villages(Guid? regionId, int page, int limit);//With Region Id

        Task<IDataResult<BaseDto<Get>>> Villages(Guid cityId, Guid? regionId, int page, int limit);//With Region Id
        Task<IDataResult<BaseDto<GetClinics>>> Clinics(string? location, Guid? cityId, Guid? regionId, Guid? villageId, Guid? partnerId, int page, int limit); // closer
        Task<IDataResult<BaseDto<ClinicsDetailed>>> Clinics(Guid? villageId, int page, int limit); // Clinics

        Task<IResult> DeleteCity(Guid id); // Delete all Resgion and Villages with city
        Task<IResult> DeleteRegion(Guid id); // delete all Villages with Resgion
        Task<IResult> DeleteVillage(Guid id); // delete village
        Task<IResult> DeleteClinic(Guid id); // delete clinic
    }
}