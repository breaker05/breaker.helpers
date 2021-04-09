using Breaker.Services.Models.Google;
using Geocoding.Google;
using System.Linq;
using System.Threading.Tasks;

namespace Breaker.Services.Google
{
    public class GoogleGeocodeeClient : IGoogleGeocodeClient
    {
        private readonly GoogleGeocoder _geocoder;

        public GoogleGeocodeeClient(GoogleGeocodeCoreOptions options)
        {
            _geocoder = new GoogleGeocoder(options.ApiKey);
        }

        public async Task<GeoAddress> Geocode(string address)
        {
            var googleAddresses = await _geocoder.GeocodeAsync(address);

            if (!googleAddresses.Any())
            {
                throw new GeocodeServiceException(
                    $"No results for this address : {address}"
                );
            }

            var formattedAddress = googleAddresses.First();
            var coordinates = googleAddresses.First();
            var street = googleAddresses.Select(a => a[GoogleAddressType.Route]).First();
            var streetNumber = googleAddresses.Select(a => a[GoogleAddressType.StreetNumber]).First();
            var city = googleAddresses.Select(a => a[GoogleAddressType.Locality]).First();
            var country = googleAddresses.Select(a => a[GoogleAddressType.Country]).First();
            var zipCode = googleAddresses.Select(a => a[GoogleAddressType.PostalCode]).First();
            var department = googleAddresses.Select(a => a[GoogleAddressType.AdministrativeAreaLevel2]).First();

            return new GeoAddress
            {
                Address = formattedAddress?.FormattedAddress,
                Lat = coordinates.Coordinates.Latitude,
                Lng = coordinates.Coordinates.Longitude,
                Street = street?.LongName,
                StreetNumber = streetNumber?.LongName,
                City = city?.LongName,
                Department = department?.LongName,
                DepartmentCode = zipCode?.LongName.Substring(0, 2),
                Country = country?.LongName,
                ZipCode = zipCode?.LongName,
            };
        }
    }
}
