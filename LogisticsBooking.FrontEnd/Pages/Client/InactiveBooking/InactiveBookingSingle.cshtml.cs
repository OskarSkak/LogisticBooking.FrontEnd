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
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.Commands;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.InactiveBooking
{
    public class InactiveBookingSingleModel : PageModel
    {
        private IInactiveBookingDataService _inactiveBookingDataService;
        [BindProperty(SupportsGet = true)] public InactiveBookingViewModel Booking { get; set; }
        
        public InactiveBookingSingleModel(IInactiveBookingDataService _bookingDataService)
        {
            _inactiveBookingDataService = _bookingDataService;
            Booking = new InactiveBookingViewModel();
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            Booking = new InactiveBookingViewModel();
            Booking = await _inactiveBookingDataService.GetInactiveBookingById(Guid.Parse(id));
            return Page();
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var response = _inactiveBookingDataService.DeleteInactiveBooking(Guid.Parse(id));
            if (response.IsCompletedSuccessfully) return new RedirectToPageResult("InactiveBookingOverview");
            else return new RedirectToPageResult("InactiveBookingOverviewa"); //TODO: Fix when error page designed
        }

        public async Task<IActionResult> OnPostUpdate(InactiveBookingViewModel Booking, 
            DateTime PageBookingTime, int PageTotalPallets, int PagePort,
            int PageExternalId, string PageInternalId)
        {
            var inactiveBooking = await _inactiveBookingDataService.GetInactiveBookingById(Guid.Parse(PageInternalId));
            inactiveBooking.Port = PagePort; //No check as i think the port can be 0 in reality
            if (PageBookingTime != default(DateTime)) inactiveBooking.BookingTime = PageBookingTime;
            if (PageTotalPallets != 0) inactiveBooking.TotalPallets = PageTotalPallets;
            if (PageExternalId != 0) inactiveBooking.ExternalId = PageExternalId;

            for (int i = 0; i < inactiveBooking.InactiveOrders.Count; i++)
            {
                if(!string.IsNullOrWhiteSpace(Booking.InactiveOrders[i].ExternalId)) inactiveBooking.InactiveOrders[i].ExternalId = Booking.InactiveOrders[i].ExternalId;
                inactiveBooking.InactiveOrders[i].Comment = Booking.InactiveOrders[i].Comment; //No check: Comment should be able to be left blank i think
                if(Booking.InactiveOrders[i].BottomPallets != 0) inactiveBooking.InactiveOrders[i].BottomPallets = Booking.InactiveOrders[i].BottomPallets;
                if(!string.IsNullOrWhiteSpace(Booking.InactiveOrders[i].InOut)) inactiveBooking.InactiveOrders[i].InOut = Booking.InactiveOrders[i].InOut;
                if(!string.IsNullOrWhiteSpace(Booking.InactiveOrders[i].OrderNumber)) inactiveBooking.InactiveOrders[i].OrderNumber = Booking.InactiveOrders[i].OrderNumber;
                if(Booking.InactiveOrders[i].TotalPallets != 0) inactiveBooking.InactiveOrders[i].TotalPallets = Booking.InactiveOrders[i].TotalPallets;
                if(Booking.InactiveOrders[i].WareNumber != 0) inactiveBooking.InactiveOrders[i].WareNumber = Booking.InactiveOrders[i].WareNumber;
            }

            //var result = _inactiveBookingDataService.UpdateInactiveBooking(inactiveBooking);
            
            return new RedirectToPageResult("InactiveBookingOverview");
        } 
    }
    
}

