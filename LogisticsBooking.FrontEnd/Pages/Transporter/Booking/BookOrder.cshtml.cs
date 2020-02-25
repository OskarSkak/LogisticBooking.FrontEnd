using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class BookOrder : PageModel
    {
        private readonly IUtilBookingDataService _utilBookingDataService;
        private readonly IScheduleDataService _scheduleDataService;
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly IBookingValidationDataService _bookingValidationDataService;

        [BindProperty] public BookingViewModel BookingViewModel { get; set; }

        [TempData] public String ScheduleAvailableMessage { get; set; }

        [TempData] public string ModelStateMessage { get; set; }

        public bool ShowMessage =>
            !String.IsNullOrEmpty(ScheduleAvailableMessage) || !String.IsNullOrEmpty(ModelStateMessage);

        public BookOrder(IUtilBookingDataService utilBookingDataService, IScheduleDataService scheduleDataService,
            IMasterScheduleDataService masterScheduleDataService , IBookingValidationDataService bookingValidationDataService)
        {
            _utilBookingDataService = utilBookingDataService;
            _scheduleDataService = scheduleDataService;
            _masterScheduleDataService = masterScheduleDataService;
            _bookingValidationDataService = bookingValidationDataService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(BookingViewModel bookingViewModel, DateTime bookingTime)
        {
            // check also if there is more empty intervals
            var validationMessage = await _bookingValidationDataService.CheckIfDateIsAllowed(new BookingDayValidationCommand{BookingDay = bookingTime});

            if (!validationMessage.IsSuccess)
            {
                ModelStateMessage = validationMessage.Errors.FirstOrDefault();
                return Page(); 
            }
            
            bookingViewModel.BookingTime = bookingTime;
            await UpdateBookingInformation(bookingViewModel);

            AddBookingViewModelToSession();

            // If all is success - navigate to order information page
            return new RedirectToPageResult("orderinformation" , new {culture = CultureInfo.CurrentCulture.Name});
        }


        private async Task UpdateBookingInformation(BookingViewModel bookingViewModel)
        {
            //Getting the next Booking number
            var externalBookingId = await _utilBookingDataService.GetBookingNumber();

            // Adds remaining pallets to the BookingViewModel 
            BookingViewModel.PalletsRemaining = bookingViewModel.TotalPallets;

            // Set the External Booking ID
            BookingViewModel.ExternalId = externalBookingId.bookingid;

            BookingViewModel.BookingTime = bookingViewModel.BookingTime;
        }

        private void AddBookingViewModelToSession()
        {
            HttpContext.Session.Remove(GetUserId());
            // Set the updated BookingViewModel to the session. The key is the current logged in user. 
            HttpContext.Session.SetObject("booking", BookingViewModel);
        }

        private string GetUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
        }
    }
}