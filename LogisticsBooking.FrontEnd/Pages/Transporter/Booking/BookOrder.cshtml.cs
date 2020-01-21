using System;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookOrder : PageModel
    {
        private readonly IUtilBookingDataService _utilBookingDataService;
        private readonly IScheduleDataService _scheduleDataService;

        [BindProperty]
        public BookingViewModel BookingViewModel { get; set; }

        [TempData]
        public String ScheduleAvailableMessage { get; set; }

        [TempData]
        public string ModelStateMessage { get; set; }
        public bool ShowMessage => !String.IsNullOrEmpty(ScheduleAvailableMessage) || !String.IsNullOrEmpty(ModelStateMessage) ;
        
        public BookOrder(IUtilBookingDataService utilBookingDataService , IScheduleDataService scheduleDataService)
        {
            _utilBookingDataService = utilBookingDataService;
            _scheduleDataService = scheduleDataService;
        }
        
        public void OnGet()
        {
            
            
        }

        public async Task<IActionResult> OnPostAsync(BookingViewModel bookingViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            await UpdateBookingInformation(bookingViewModel);
            
            AddBookingViewModelToSession();
            
            
            // If all is success - navigate to order information page
            return new RedirectToPageResult("orderinformation");
        }


        private async Task UpdateBookingInformation(BookingViewModel bookingViewModel)
        {
            //Getting the next Booking number
            var externalBookingId  = await _utilBookingDataService.GetBookingNumber();
            
            // Adds remaining pallets to the BookingViewModel 
            BookingViewModel.PalletsRemaining = bookingViewModel.TotalPallets;
            
            // Set the External Booking ID
            BookingViewModel.ExternalId = externalBookingId.bookingid;
        }

        private void AddBookingViewModelToSession()
        {
            // Set the updated BookingViewModel to the session. The key is the current logged in user. 
            HttpContext.Session.SetObject(GetUserId() ,BookingViewModel);
        }
        
        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
        }

        
    }
    
   
}
