using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.DeletedBooking.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class DeletedBookingOverviewModel : PageModel
    {
        private readonly IDeletedBookingDataService _deletedBookingDataService;


        [BindProperty] public DeletedBookingsListViewModel BookingsListViewModel { get; set; }
        [BindProperty] public DeletedBookingsListViewModel DeletedBookingsListViewModel { get; set; }


        public DeletedBookingOverviewModel(IDeletedBookingDataService deletedBookingDataService)
        {
            //_bookingDataService = bookingDataService;
            _deletedBookingDataService = deletedBookingDataService;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            BookingsListViewModel = await _deletedBookingDataService.GetDeletedBookings();
            
            foreach (var booking in BookingsListViewModel.DeletedBookings)
            {
                if (String.IsNullOrWhiteSpace(booking.TransporterName)) booking.TransporterName = "N/A";
                if (String.IsNullOrWhiteSpace(booking.Email)) booking.Email = "N/A";

                booking.ActualArrival = default(DateTime).Add(booking.ActualArrival.Value.TimeOfDay);
                booking.EndLoading = default(DateTime).Add(booking.EndLoading.Value.TimeOfDay);
                booking.StartLoading = default(DateTime).Add(booking.StartLoading.Value.TimeOfDay);

                foreach (var order in booking.OrdersListViewModel)
                {
                    if (String.IsNullOrWhiteSpace(order.CustomerNumber)) order.CustomerNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.OrderNumber)) order.OrderNumber = "N/A";
                    if (String.IsNullOrWhiteSpace(order.InOut)) order.InOut = "N/A";
                }
            }

            return Page();
        }
        
        /*
        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            var result = await _deletedBookingData
            ResponseMessage = "Transportøren er slettet korrekt";
            return new RedirectToPageResult("./Transporters");
        }
        */
        
        //TODO: ERROR HANDLING AFTER AGREEMENT

        public async Task<IActionResult> OnPostDeleteAsync(string InternalIdView)
        {
            var InternalIdView_Guid = Guid.Parse(InternalIdView);
            var result = _deletedBookingDataService.DeleteBooking(InternalIdView_Guid);

            //return !result.IsCompletedSuccessfully ? new RedirectToPageResult("./error") : new RedirectToPageResult("./DeletedBookingOverview"); 
            //TODO: INDSÆT KORREKT REDIRECT
            return new RedirectToPageResult("./DeletedBookingOverview");
        }

        public async Task<IActionResult> OnPostUpdateAsync(string InternalIdView)
        {
            return new RedirectToPageResult("./DeletedBookingOverview");

        }
        
        public async Task<IActionResult> OnPostGoToAsync(string InternalIdView)
        {
            return new RedirectToPageResult("./DeletedBookingOverview");
        }
    }
}
