using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class IntervalDataService : BaseDataService ,  IIntervalDataService
    {
        private string baseurl;
        
        public IntervalDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/intervals/";
        }

        public async Task<IntervalViewModel> GetIntetvalById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<IntervalViewModel>(result);
        }
    }
}