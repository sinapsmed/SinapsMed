using Core.Entities.DTOs;
using Core.Utilities.Results.Abstract;

namespace Core.Helpers.Abstract
{
    public interface IGeoLocationService
    {
        Task<IDataResult<GeoLocation>> GetGeoLocation(string ipAddress);
    }
}