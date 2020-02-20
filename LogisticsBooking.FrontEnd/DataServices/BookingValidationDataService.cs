using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace LogisticsBooking.FrontEnd.DataServices
{
    public class BookingValidationDataService : BaseDataService ,  IBookingValidationDataService
    {
        private string baseurl;
        
        public BookingValidationDataService(IHttpContextAccessor httpContextAccessor, IOptions<BackendServerUrlConfiguration> config) : base(httpContextAccessor, config)
        {
            baseurl = _APIServerURL + "/api/bookings/validate/";
        }

        public async Task<ValidationMessage> CheckIfDateIsAllowed(BookingDayValidationCommand bookingDayValidationCommand)
        {
            var endpoint = baseurl + "bookingday";   
        
            var response = await PutAsync(endpoint, bookingDayValidationCommand);
            
            var message =  await TryReadAsync<ValidationMessage>(response);
            return message;

        }
        
        public async Task<ValidationMessage> CheckIfSuppliersOverlap(BookingSuppliersOverlapValidationCommand bookingSuppliersOverlapValidationCommand)
        {
            var endpoint = baseurl + "suppliers/overlap";   
        
            var response = await PutAsync(endpoint, bookingSuppliersOverlapValidationCommand);
            
            var message =  await TryReadAsync<ValidationMessage>(response);
            return message;

        }

    }
}