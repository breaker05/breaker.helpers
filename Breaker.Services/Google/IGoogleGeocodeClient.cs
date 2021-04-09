using Breaker.Services.Models.Google;
using System.Threading.Tasks;

namespace Breaker.Services.Google
{
    public interface IGoogleGeocodeClient
    {
        Task<GeoAddress> Geocode(string address);
    }
}
