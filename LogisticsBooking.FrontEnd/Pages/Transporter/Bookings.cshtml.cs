using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    public class Bookings : PageModel
    {
        private readonly ITransporterBookingsDataService _transporterBookingsDataService;
        private readonly IBookingDataService _bookingDataService;
        private readonly IUserUtility _userUtility;

        public Bookings(ITransporterBookingsDataService transporterBookingsDataService , IBookingDataService bookingDataService , IUserUtility userUtility)
        {
            _transporterBookingsDataService = transporterBookingsDataService;
            _bookingDataService = bookingDataService;
            _userUtility = userUtility;
        }
        
        public void OnGet()
        {
            
        }

        public async Task<JsonResult> OnGetBookings()
        {
            var result =
                await _transporterBookingsDataService.GetBookingsByTransporter( _userUtility.GetCurrentUserId());
            
            var json = new JsonResult(result);
            return json;
        }
        
        public async Task<IActionResult> OnGetCustom(DateTime start, DateTime end)
        {

            var bookings = await _transporterBookingsDataService.GetBookingsByTransporterBetweenDates(new GetBookingsByTransporterBetweenDatesQuery
            {
                FromDate = start,
                ToDate = end,
                TransporterId = _userUtility.GetCurrentUserId()
            });

            var json = new JsonResult(bookings);

            return json;
        }
    }
}