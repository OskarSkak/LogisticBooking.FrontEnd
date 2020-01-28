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
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.InactiveBooking
{
    public class InactiveBookingOverviewModel : PageModel
    {
        private readonly IInactiveBookingDataService _inactiveBookingDataService;

        [BindProperty] public InactiveBookingListViewModel InactiveBookingListViewModel { get; set; } 


        public InactiveBookingOverviewModel(IInactiveBookingDataService bookingDataService)
        {
            _inactiveBookingDataService = bookingDataService;
            InactiveBookingListViewModel = _inactiveBookingDataService.GetInactiveBookings().Result;
        }

        public async void OnGetAsync()
        {
            
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(string InternalIdView)
        {
            var InternalIdView_Guid = Guid.Parse(InternalIdView);
            var result = _inactiveBookingDataService.DeleteInactiveBooking(InternalIdView_Guid);
            //TODO: Error handling after agreement
            return result.IsCompletedSuccessfully ? new RedirectToPageResult("Pages/Client/InactiveBooking/InactiveBookingOverview.cshtml") : new RedirectToPageResult("ERROR PAGE");
        }
        
        public async Task<IActionResult> OnPostGoToAsync(string InternalIdView)
        {
            return new RedirectToPageResult("InactiveBookingSingle", InternalIdView);        
        }
    }
}