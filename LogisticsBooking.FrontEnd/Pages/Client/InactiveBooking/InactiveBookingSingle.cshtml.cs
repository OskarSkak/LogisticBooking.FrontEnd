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
            return new RedirectToPageResult("InactiveBookingOverview"); //TODO: Fix when error page designed
        }

        public async Task<IActionResult> OnPostUpdate(InactiveBookingViewModel Booking)
        {
            var inactiveBooking = await _inactiveBookingDataService.GetInactiveBookingById(Booking.InternalId);
            inactiveBooking.Port = Booking.Port; //No check as i think the port can be 0 in reality
            if (Booking.BookingTime != default(DateTime) && Booking.BookingTime != null) inactiveBooking.BookingTime = Booking.BookingTime;
            if (Booking.TotalPallets != 0) inactiveBooking.TotalPallets = Booking.TotalPallets;
            if (Booking.ExternalId!= 0) inactiveBooking.ExternalId = Booking.ExternalId;

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

            var result = _inactiveBookingDataService.UpdateInactiveBookingWithOrders(UpdateInactiveBookingWithOrdersCommand.GenerateCommand(inactiveBooking));
            
            return new RedirectToPageResult("InactiveBookingOverview");
        }

        public async Task<IActionResult> OnPostCreate(InactiveBookingViewModel Booking)
        {
            var booking = new BookingViewModel
            {

                BookingTime = Booking.BookingTime.Value,
                TotalPallets = Booking.TotalPallets, 
                Port = Booking.Port, 
                ExternalId = Booking.ExternalId, 
                InternalId = Booking.InternalId,
                OrdersListViewModel = new List<OrderViewModel>()
            };
            
            foreach (var inactiveOrder in Booking.InactiveOrders)
            {
                booking.OrdersListViewModel.Add(new OrderViewModel
                {
                    BookingId = booking.InternalId, 
                    BottomPallets = inactiveOrder.BottomPallets,
                    Comment = inactiveOrder.Comment, 
                    ExternalId = inactiveOrder.ExternalId, 
                    InOut = inactiveOrder.InOut, 
                    OrderNumber = inactiveOrder.OrderNumber, 
                    TotalPallets = inactiveOrder.TotalPallets, 
                    WareNumber = inactiveOrder.WareNumber
                });
            }
            
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            TempData.Set(id, booking);
            return new RedirectToPageResult("Client/Bookings/select_time");
        }
    }
    
}

