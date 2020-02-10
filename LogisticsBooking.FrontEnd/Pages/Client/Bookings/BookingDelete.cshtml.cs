using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class BookingDeleteModel : PageModel
    {
        private IBookingDataService _bookingDataService;
        private readonly ILogger<BookingDeleteModel> _logger;

        [BindProperty] 
        public BookingViewModel Booking { get; set; }
        
        public BookingDeleteModel(IBookingDataService bookingDataService , ILogger<BookingDeleteModel> logger)
        {
            _bookingDataService = bookingDataService;
            _logger = logger;
        }

        public async Task OnGetAsync(string id)
        {
            Booking = await _bookingDataService.GetBookingById(Guid.Parse(id));
            
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var result = await _bookingDataService.DeleteBooking(Guid.Parse(id));

            if (!result.IsSuccesfull) return new RedirectToPageResult("Error");
            return new RedirectToPageResult("BookingOverviewAdmin");
        }
        

       
        
    }
}
