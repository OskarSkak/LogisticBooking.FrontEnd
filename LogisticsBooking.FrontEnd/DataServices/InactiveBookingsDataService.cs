using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.Commands;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices.Utilities
{
    public class InactiveBookingsDataService : BaseDataService, IInactiveBookingDataService
    {
        private string baseurl;
        
        public InactiveBookingsDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/inactiveBookings/";
        }

        public async Task<Response> UpdateInactiveBooking(UpdateInactiveBookingCommand cmd)
        {
            var response = await PutAsync(baseurl, cmd);
            if (response.IsSuccessStatusCode) return Response.Succes();
            if (response.Content == null) return Response.Unsuccesfull(response, response.ReasonPhrase);
            var errorMsg = await response.Content.ReadAsStringAsync();
            return Response.Unsuccesfull(response,errorMsg);
        }
        
        public async Task<Response> DeleteInactiveBooking(Guid id)
        {
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode) return new Response(true);
            return Response.Unsuccesfull();
        }

        public async Task<InactiveBookingViewModel> GetInactiveBookingById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<InactiveBookingViewModel>(result);
        }

        public async Task<InactiveBookingListViewModel> GetInactiveBookings()
        {
            var response = await GetAsync(baseurl);
            var result = await TryReadAsync<InactiveBookingListViewModel>(response);
            return result;
        }

        public async Task<Response> UpdateInactiveBookingWithOrders(UpdateInactiveBookingWithOrdersCommand cmd)
        {
            var endUrl = baseurl + "InactiveOrders/";
            var response = await PutAsync(endUrl, cmd);
            if (response.Content == null) return Response.Unsuccesfull(response, response.ReasonPhrase);
            var errorMsg = await response.Content.ReadAsStringAsync();
            return Response.Unsuccesfull(response,errorMsg);
        }
    }
}