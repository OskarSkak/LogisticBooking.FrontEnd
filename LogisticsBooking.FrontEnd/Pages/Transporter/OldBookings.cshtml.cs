using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoreLinq.Extensions;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    public class OldBookings : PageModel
    {
        
        private readonly ITransporterBookingsDataService _transporterBookingsDataService;
        private readonly IUserUtility _userUtility;
        
        [BindProperty] 
        public BookingsListViewModel BookingsListViewModel{ get; set; }
        
        public List<IntervalViewModel> IntervalViewModels { get; set; }

        public OldBookings(ITransporterBookingsDataService transporterBookingsDataService ,  IUserUtility userUtility)
        {
           
            _transporterBookingsDataService = transporterBookingsDataService;
            _userUtility = userUtility;
        }
        public void OnGet()
        {
           
        }

        public async Task<JsonResult> OnGetOldBookings()
        {
            var loggedIn =  _userUtility.GetCurrentUserId();





            var result = await _transporterBookingsDataService.GetOldBookingsByTransporter(loggedIn);
            
            var json = new JsonResult(result);
            return json;
        }
    }
}