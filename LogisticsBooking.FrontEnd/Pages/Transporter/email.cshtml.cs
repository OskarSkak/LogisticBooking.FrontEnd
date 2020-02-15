using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    
    
    public class email : PageModel
    {
        private readonly IBookingDataService _bookingDataService;

        public BookingViewModel Booking { get; set; }


        public email(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
        }
        
        public async Task OnGet()
        {
            Booking = await _bookingDataService.GetBookingById(Guid.Parse("3f6793b9-fa5e-4e94-aebb-04eb4e4d6cb8"));

        }
    }
}