using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class TransporterBookingsDataService : BaseDataService , ITransporterBookingsDataService
    {
        
        
        private string baseurl;
        public TransporterBookingsDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/transporters/oldbookings/";
        }

        public async Task<BookingsListViewModel> GetOldBookingsByTransporter(Guid TransporterId)
        {
            var endpoint = baseurl + TransporterId;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<BookingsListViewModel>(result);
        }

        public async Task<BookingsListViewModel> GetBookingsByTransporter(Guid TransporterId)
        {
            var endpoint = _APIServerURL + "/api/transporters/bookings/" + TransporterId;
            var result = await GetAsync(endpoint);
            return await TryReadAsync<BookingsListViewModel>(result);
        }
        
    }
}