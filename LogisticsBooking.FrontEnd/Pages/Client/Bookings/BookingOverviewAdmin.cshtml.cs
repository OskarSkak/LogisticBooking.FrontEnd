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

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class BookingOverviewAdminModel : PageModel
    {
        private readonly IBookingDataService _bookingDataService;


        [BindProperty] public BookingsListViewModel BookingsListViewModel { get; set; } 
        [BindProperty]
        public DateTime Start { get; set; }
        [BindProperty]
        public DateTime End { get; set; }

        public BookingOverviewAdminModel(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
            
        }

        public  void OnGetAsync()
        {

          
        }
        
        public async Task<IActionResult> OnGetToday()
        {
            var bookings = await _bookingDataService.GetBookingsInbetweenDates(DateTime.Today  , DateTime.Today);

            var json = new JsonResult(bookings);

            return json;
        }

        public async Task<IActionResult> OnGetAll()
        {
            var bookings =
                await _bookingDataService.GetBookings();

            var json = new JsonResult(bookings);

            return json;
        }

        public async Task<IActionResult> OnGetEdit(string id)
        {
            Console.WriteLine(id);
            return new RedirectToPageResult("BookingSingle", new {id = id});
        }
        
        public async Task<IActionResult> OnGetCustom(DateTime start, DateTime end)
        {
            Start = start;
            End = end;

            var bookings = await _bookingDataService.GetBookingsInbetweenDates(start,end);

            var json = new JsonResult(bookings);

            return json;
        }
    }
}