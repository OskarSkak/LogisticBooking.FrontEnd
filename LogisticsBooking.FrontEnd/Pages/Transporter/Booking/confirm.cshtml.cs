using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class confirm : PageModel
    {
        private readonly IBookingDataService _bookingDataService;


        public confirm(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
        }
        public async Task<IActionResult> OnGet(BookingViewModel bookingOrderViewModel)
        {

            return Page();

        }
    }
}