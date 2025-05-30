using Core.Entities.DTOs;
using Core.Helpers.Abstract;
using Core.Utilities.Results.Abstract;
using Core.Utilities.Results.Concrete.ErrorResult;
using Core.Utilities.Results.Concrete.SuccessResult;
using Newtonsoft.Json;

namespace Core.Helpers.Concrete
{
    public class GeoLocationManager : IGeoLocationService
    {
        public async Task<IDataResult<GeoLocation>> GetGeoLocation(string ipAddress)
        {
            string url = $"http://ip-api.com/json/{ipAddress}?fields=status,country,city,lat,lon";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    GeoLocation location = JsonConvert.DeserializeObject<GeoLocation>(jsonResponse); 
                    
                    return new SuccessDataResult<GeoLocation>(location);
                }
                return new ErrorDataResult<GeoLocation>("IP konum bilgisi alınamadı.");
            }

        }
    }
}