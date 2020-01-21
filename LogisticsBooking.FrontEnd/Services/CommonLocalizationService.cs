using System.Reflection;
using LogisticsBooking.FrontEnd.Resources;
using Microsoft.Extensions.Localization;

namespace LogisticsBooking.FrontEnd.Services
{
    public class CommonLocalizationService
    {
        private readonly IStringLocalizer localizer;
        public CommonLocalizationService(IStringLocalizerFactory factory)
        {
            var assemblyName = new AssemblyName(typeof(CommonResources).GetTypeInfo().Assembly.FullName);
            localizer = factory.Create(nameof(CommonResources), assemblyName.Name);
        }
 
        public string Get(string key)
        {
            return localizer[key];
        }

        
    }
}