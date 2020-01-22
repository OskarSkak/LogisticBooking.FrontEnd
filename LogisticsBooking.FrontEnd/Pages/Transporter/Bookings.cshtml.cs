using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    public class Bookings : PageModel
    {
        private readonly ITransporterBookingsDataService _transporterBookingsDataService;
        private readonly IUserUtility _userUtility;

        public Bookings(ITransporterBookingsDataService transporterBookingsDataService , IUserUtility userUtility)
        {
            _transporterBookingsDataService = transporterBookingsDataService;
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
    }
}