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
        private readonly IBookingDataService _bookingDataService;
        private readonly ITransporterDataService _transporterDataService;
        private readonly IIntervalDataService _intervalDataService;
        private readonly IUserUtility _userUtility;
        
        [BindProperty] 
        public BookingsListViewModel BookingsListViewModel{ get; set; }
        
        public List<IntervalViewModel> IntervalViewModels { get; set; }

        public OldBookings(IBookingDataService bookingDataService , ITransporterDataService transporterDataService, IIntervalDataService intervalDataService , IUserUtility userUtility)
        {
            _bookingDataService = bookingDataService;
            _transporterDataService = transporterDataService;
            _intervalDataService = intervalDataService;
            _userUtility = userUtility;
        }
        public async Task<IActionResult> OnGet()
        {
            
            

            var loggedIn = await _transporterDataService.GetTransporterById(_userUtility.GetCurrentUserId());
            
            var allBookings = await _bookingDataService.GetBookings();
            var intervalViewModels = new List<IntervalViewModel>();
            BookingsListViewModel = new BookingsListViewModel();
            foreach (var booking in allBookings.Bookings)
            {
                if (booking.TransporterId == loggedIn.TransporterId)
                {
                    BookingsListViewModel.Bookings.Add(booking);
                    intervalViewModels.Add(booking.Interval);
                }
            }

            IntervalViewModels = intervalViewModels.DistinctBy(x => x.IntervalId).ToList();
            return Page();
        }
    }
}