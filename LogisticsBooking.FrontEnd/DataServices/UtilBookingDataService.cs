using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class UtilBookingDataService : BaseDataService , IUtilBookingDataService
    {
        
        private string baseurl;
        
        public UtilBookingDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/utilbooking/";
        }

        public async Task<UtilBooking> GetBookingNumber()
        {
            var endpoint = baseurl;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<UtilBooking>(result);
        }
    }
}