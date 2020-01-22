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
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class BookingSingleModel : PageModel
    {
        private IBookingDataService bookingDataService;
        [BindProperty] public BookingViewModel Booking { get; set; }
        [BindProperty] public int ArrivalMinute { get; set; }
        [BindProperty] public int ArrivalHour { get; set; }
        
        [BindProperty] public int StartMinute { get; set; }
        [BindProperty] public int StartHour { get; set; }
        
        [BindProperty] public int EndMinute { get; set; }
        [BindProperty] public int EndHour { get; set; }
        
        [TempData]
        public String Message { get; set; }

        public bool MessegeIsSet => !String.IsNullOrEmpty(Message);
        
        public BookingSingleModel(IBookingDataService _bookingDataService)
        {
            bookingDataService = _bookingDataService;
        }

        public async Task OnGetAsync(string id)
        {
            Booking = new BookingViewModel();
            Booking = await bookingDataService.GetBookingById(Guid.Parse(id));
            Booking = BookingUtil.RemoveDates(Booking);
            ArrivalHour = Booking.ActualArrival.Hour;
            ArrivalMinute = Booking.ActualArrival.Minute;

            StartMinute = Booking.StartLoading.Minute;
            EndMinute = Booking.EndLoading.Minute;
            StartHour = Booking.StartLoading.Hour;
            EndHour = Booking.EndLoading.Hour;
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var guid = Guid.Parse(id);

            var result = await bookingDataService.DeleteBooking(guid);
            
            return new RedirectToPageResult("BookingOverViewAdmin");
        }

        public async Task<IActionResult> OnPostUpdate(DateTime ViewBookTime,
            int ViewPallets, int ViewPort, int ActualArrivalHour,
            int ActualArrivalMinute, int startHour, int startMinute,
            int endHour, int endMinute, Guid ViewBookingId)
        {
            
            var booking = new UpdateBookingCommand()
            {
                BookingTime = ViewBookTime, 
                TotalPallets = ViewPallets, 
                Port = ViewPort, 
                ActualArrival = GeneralUtil.TimeFromHourAndMinute(ActualArrivalHour, ActualArrivalMinute), 
                StartLoading = GeneralUtil.TimeFromHourAndMinute(startHour, startMinute), 
                EndLoading = GeneralUtil.TimeFromHourAndMinute(endHour, endMinute), 
                InternalId = ViewBookingId
            };

            var result = await bookingDataService.UpdateBooking(booking);

            if (result.IsSuccesfull)
            {
                Message = "Bookingen er opdateret korrekt";
                return new RedirectToPageResult("BookingSingle" , new {id = ViewBookingId});
            }

            return new RedirectToPageResult("Error");
        }
        
        
    }
    
}

