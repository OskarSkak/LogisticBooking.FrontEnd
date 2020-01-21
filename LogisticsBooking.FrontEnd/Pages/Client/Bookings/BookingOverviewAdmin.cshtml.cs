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
    public class BookingOvervieAdminwModel : PageModel
    {
        private readonly IBookingDataService _bookingDataService;


        [BindProperty] public BookingsListViewModel BookingsListViewModel { get; set; } 


        public BookingOvervieAdminwModel(IBookingDataService bookingDataService)
        {
            _bookingDataService = bookingDataService;
            
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {

            BookingsListViewModel = await _bookingDataService.GetBookings();
            
            
            foreach (var booking in BookingsListViewModel.Bookings)
            {
                if (String.IsNullOrWhiteSpace(booking.TransporterName)) booking.TransporterName = "N/A";
                if (String.IsNullOrWhiteSpace(booking.Email)) booking.Email = "N/A";

                booking.ActualArrival = default(DateTime).Add(booking.ActualArrival.TimeOfDay);
                booking.EndLoading = default(DateTime).Add(booking.EndLoading.TimeOfDay);
                booking.StartLoading = default(DateTime).Add(booking.StartLoading.TimeOfDay);

                foreach (var order in booking.OrdersListViewModel)
                {
                    if (String.IsNullOrWhiteSpace(order.CustomerNumber)) order.CustomerNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.OrderNumber)) order.OrderNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.InOut)) order.InOut = "N/A";
                }
            }

            return Page();
        }
    }
}