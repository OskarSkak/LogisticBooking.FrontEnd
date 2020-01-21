using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.DeletedBooking.CommandModels;
using LogisticsBooking.FrontEnd.DataServices.Models.DeletedBooking.ViewModels;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices.Utilities
{
    public class DeletedBookingDataService : BaseDataService, IDeletedBookingDataService
    {
        private string _url;
        private string baseurl;
        
        public DeletedBookingDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/deletedBookings/";
        }

        public async Task<DeletedBookingsListViewModel> GetDeletedBookings()
        {
            var response = await GetAsync(baseurl);
            var result = await TryReadAsync<DeletedBookingsListViewModel>(response);
            Console.WriteLine(result);

            return result;
        }

        public async Task<DeletedBookingViewModel> GetDeletedBookingById(Guid id)
        {
            var endpoint = baseurl + id;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<DeletedBookingViewModel>(result);
        }

        public async Task<Response> UpdateDeletedBooking(UpdateDeletedBookingCommand command)
        {
            var response = await PutAsync(baseurl, command);

            if (response.IsSuccessStatusCode) return Response.Succes();
            if (response.Content == null) return Response.Unsuccesfull(response, response.ReasonPhrase);
            var errorMsg = await response.Content.ReadAsStringAsync();
            
            return Response.Unsuccesfull(response,errorMsg);
        }

        public async Task<Response> DeleteBooking(Guid id)
        {
            var endpoint = baseurl + id;
            var response = await DeleteAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                return new Response(true );
            }
            return Response.Unsuccesfull();
        }
    }
}