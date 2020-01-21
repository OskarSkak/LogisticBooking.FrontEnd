using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class DashboardDataservice : BaseDataService ,  IDashboardDataService
    {
        
        private string baseurl;
        
        public DashboardDataservice(IHttpContextAccessor httpContextAccessor,
            IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/dashboard/";
        }
        
        public async Task<DashboardViewModel> GetDashboard()
        {
            var response = await GetAsync(baseurl);
            var result = await TryReadAsync<DashboardViewModel>(response);

            return result;
        }
    }
}