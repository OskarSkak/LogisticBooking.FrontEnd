using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter
{
    public class ReservedOrdersIndexModel : PageModel
    {
        private IBookingDataService BookingDataService { get; set; }
        [BindProperty] public BookingViewModel Booking { get; set; }
        [BindProperty] public int ArrivalHour { get; set; }
        [BindProperty] public int ArrivalMinute { get; set; }
        [BindProperty] public int startHour { get; set; }
        [BindProperty] public int startMinute { get; set; }
        [BindProperty] public int endHour { get; set; }
        [BindProperty] public int endMinute { get; set; }
        
        public ReservedOrdersIndexModel(IBookingDataService _bookingDataService)
        {
            BookingDataService = _bookingDataService;
        }
        
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var guid = Guid.Parse(id);
            Booking = await BookingDataService.GetBookingById(guid);
            ArrivalHour = Booking.ActualArrival.Hour;
            ArrivalMinute = Booking.ActualArrival.Minute;

            startHour = Booking.StartLoading.Hour;
            startMinute = Booking.StartLoading.Minute;

            endHour = Booking.EndLoading.Hour;
            endMinute = Booking.EndLoading.Minute;
            
            return Page();
        }
        
        public async Task<IActionResult> OnPostDelete(string id)
        {
            var guid = Guid.Parse(id);

            var result = await BookingDataService.DeleteBooking(guid);
            
            return new RedirectToPageResult("ReservedBookings");
        }

        public async Task<IActionResult> OnPostUpdate(DateTime ViewBookTime,
            int ViewPallets, int ViewPort, int ActualArrivalHour,
            int ActualArrivalMinute, int startHour, int startMinute,
            int endHour, int endMinute, Guid ViewBookingId)
        {
            
            var booking = new BookingViewModel
            {
                BookingTime = ViewBookTime, 
                TotalPallets = ViewPallets, 
                Port = ViewPort, 
                ActualArrival = GeneralUtil.TimeFromHourAndMinute(ActualArrivalHour, ActualArrivalMinute), 
                StartLoading = GeneralUtil.TimeFromHourAndMinute(startHour, startMinute), 
                EndLoading = GeneralUtil.TimeFromHourAndMinute(endHour, endMinute), 
                InternalId = ViewBookingId
            };

            var result = await BookingDataService.UpdateBooking(CreateUpdateBookingCommand(booking));
            
            if(result.IsSuccesfull) return new RedirectToPageResult("ReservedBookings");

            return new RedirectToPageResult("Error");
        }
        
        private UpdateBookingCommand CreateUpdateBookingCommand(BookingViewModel bookingToUpdate)
        {
            return new UpdateBookingCommand
            {
                Email = bookingToUpdate.Email,
                Port = bookingToUpdate.Port,
                ActualArrival = bookingToUpdate.ActualArrival,
                BookingTime = bookingToUpdate.ActualArrival,
                EndLoading = bookingToUpdate.ActualArrival,
                ExternalId = bookingToUpdate.ExternalId,
                InternalId = bookingToUpdate.InternalId,
                StartLoading = bookingToUpdate.StartLoading,
                TotalPallets = bookingToUpdate.TotalPallets,
                TransporterId = bookingToUpdate.TransporterId,
                TransporterName = bookingToUpdate.TransporterName
            };
        }
    }
}