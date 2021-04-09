using Breaker.Services.Google;
using Breaker.Services.Models.Google;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Breaker.Services.Configuration
{
    public static class IServiceCollectionExtensions
    {
        private const string Identifier = "GoogleGeocode";

        public static IServiceCollection AddGoogleGeocodeCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();

            var settings = configuration.GetSection(Identifier);
            services.Configure<GoogleGeocodeCoreOptions>(options => options.ApiKey = settings["ApiKey"]);

            services.TryAddTransient<IGoogleGeocodeClient>((sp) =>
            {
                var options = sp.GetService<IOptions<GoogleGeocodeCoreOptions>>().Value;
                return new GoogleGeocodeeClient(options);
            });

            return services;
        }
    }
}
