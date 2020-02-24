using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Customer;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class CustomerDataService : BaseDataService , ICustomerDataService
    {
        private readonly IOptions<IdentityServerConfiguration> _identityserverConfig;

        private string baseurl;
        
        public CustomerDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config , IOptions<IdentityServerConfiguration> identityserverConfig) : base(httpContextAccessor, config)
        {
            _identityserverConfig = identityserverConfig;
            baseurl = identityserverConfig.Value.IdentityServerUrl  + "/customers";
        }


        public async Task<Response> CreateCustomer(CreateCustomerCommand command)
        {
            var response = await PostAsync<CreateCustomerCommand>(baseurl, command);
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return Response.Unsuccesfull();
        
        }
    }
}