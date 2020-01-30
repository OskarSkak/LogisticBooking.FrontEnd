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
        private readonly IMasterScheduleDataService _masterScheduleDataService;

        [BindProperty]
        public BookingViewModel BookingViewModel { get; set; }

        [TempData]
        public String ScheduleAvailableMessage { get; set; }

        [TempData]
        public string ModelStateMessage { get; set; }
        public bool ShowMessage => !String.IsNullOrEmpty(ScheduleAvailableMessage) || !String.IsNullOrEmpty(ModelStateMessage) ;
        
        public BookOrder(IUtilBookingDataService utilBookingDataService , IScheduleDataService scheduleDataService , IMasterScheduleDataService masterScheduleDataService)
        {
            _utilBookingDataService = utilBookingDataService;
            _scheduleDataService = scheduleDataService;
            _masterScheduleDataService = masterScheduleDataService;
        }
        
        public void OnGet()
        {
            
            
        }

        public async Task<IActionResult> OnPostAsync(BookingViewModel bookingViewModel , DateTime bookingTime)
        {
            if (!ModelState.IsValid)
            {
                foreach (var model in ModelState)
                {
                    
                    Console.WriteLine(model);
                }
                return Page();
            }

            if (bookingTime.Date <= DateTime.Now)
            {
                ModelStateMessage = "Dagen skal værer efter idag";
                return Page();
            }


            if (! await ScheduleIsAllowed(bookingTime))
            {
                ModelStateMessage = "Der kan ikke bookes på den dag";
                return Page();
            }
            
            bookingViewModel.BookingTime = bookingTime;
            await UpdateBookingInformation(bookingViewModel);
            
            AddBookingViewModelToSession();
            
            
            // If all is success - navigate to order information page
            return new RedirectToPageResult("orderinformation");
        }

        private async Task<bool> ScheduleIsAllowed( DateTime bookingTime)
        {
            // Er der en schedule på dagen? 
            // Hvis der er allerede er en på dagen er de aktive days godkendt.
            var currentSchedule = await _scheduleDataService.GetScheduleBydate(bookingTime);
            if (currentSchedule != null)
            {
                return true;
            }

            var MasterSchedules = await _masterScheduleDataService.GetActiveMasterSchedule();
            foreach (var masterScheduleStandardViewModel in MasterSchedules.MasterScheduleStandardViewModels)
            {
                var activeDays = masterScheduleStandardViewModel.ActiveDays.FirstOrDefault(e => e.ActiveDay == bookingTime.DayOfWeek);
                if (activeDays == null)
                {
                    return false;
                }

            }

            return true;

        }


        private async Task UpdateBookingInformation(BookingViewModel bookingViewModel)
        {
            //Getting the next Booking number
            var externalBookingId  = await _utilBookingDataService.GetBookingNumber();
            
            // Adds remaining pallets to the BookingViewModel 
            BookingViewModel.PalletsRemaining = bookingViewModel.TotalPallets;
            
            // Set the External Booking ID
            BookingViewModel.ExternalId = externalBookingId.bookingid;

            BookingViewModel.BookingTime = bookingViewModel.BookingTime;
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
