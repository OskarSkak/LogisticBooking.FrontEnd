using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;

namespace LogisticsBooking.FrontEnd.Services
{
    public class RouteValueRequestCultureProvider : IRequestCultureProvider
    {
        
        private readonly CultureInfo[] _cultures;

        public RouteValueRequestCultureProvider(CultureInfo[] cultures)
        {
            _cultures = cultures;
        }
        
        public Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var defaultCulture = "da";

            var path = httpContext.Request.Path;

            if (string.IsNullOrWhiteSpace(path))
            {
                return Task.FromResult(new ProviderCultureResult(defaultCulture));
            }

            var routeValues = httpContext.Request.Path.Value.Split('/');
            if (routeValues.Count() <= 1)
            {
                return Task.FromResult(new ProviderCultureResult(defaultCulture));
            }

            if (_cultures.All(x => x.Name.ToLower() != routeValues[1].ToLower()))
            {
                return Task.FromResult(new ProviderCultureResult(defaultCulture));
            }

            return Task.FromResult(new ProviderCultureResult(routeValues[1]));
        }
    }
}