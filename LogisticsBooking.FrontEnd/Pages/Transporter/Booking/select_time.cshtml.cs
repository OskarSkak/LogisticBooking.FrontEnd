using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class select_time : PageModel
    {
        private readonly IBookingDataService _bookingDataService;
        private readonly IScheduleDataService _scheduleDataService;
        private readonly IMapper _mapper;
        private readonly IMasterScheduleDataService _masterScheduleDataService;

        [BindProperty]
        public SchedulesListViewModel SchedulesListViewModel { get; set; }
        
        [BindProperty]
        public IntervalViewModel IntervalViewModel { get; set; }
        
        [TempData]
        public string ErrorMessage { get; set; }

        public bool ShowErrorMessage => !String.IsNullOrEmpty(ErrorMessage);

        public select_time(IBookingDataService bookingDataService , IScheduleDataService scheduleDataService , IMapper mapper , IMasterScheduleDataService masterScheduleDataService)
        {
            _bookingDataService = bookingDataService;
            _scheduleDataService = scheduleDataService;
            _mapper = mapper;
            _masterScheduleDataService = masterScheduleDataService;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var currentLoggedInUserId = GetLoggedInUserId();

            // Get the current booking View Model from the session created at previous page
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>("booking");

            
            // remove the intervals that does not overlap with the suppliers time range. 
            // It is only possible to book a time with that match the selected suppliers on the orders. 


            
            // USE THIS
            SchedulesListViewModel =  await _scheduleDataService.GenerateScheduleFromCurrentBooking(new GenerateScheduleForNewBookingCommand{BookingTime = currentBooking.BookingTime, SupplierViewModels = currentBooking.SuppliersListViewModel});
            
            
            return Page();
            
        }

        


        
        public async Task<IActionResult> OnPostSelectedTime(string interval , ScheduleViewModel schedule)
        {
            var currentLoggedInUserId = GetLoggedInUserId();

            // Get the current booking View Model from the session created at previous page
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>("booking");
            

            var createBookingcommand = _mapper.Map<CreateBookingCommand>(currentBooking);
            createBookingcommand.IntervalId = Guid.Parse(interval);
            createBookingcommand.TransporterId = Guid.Parse(currentLoggedInUserId);
            createBookingcommand.IsValidated = true;
            var result = await _bookingDataService.CreateBooking(createBookingcommand);

            if (result.IsSuccesfull)
            {
                return RedirectToPage("confirm" , new {culture = CultureInfo.CurrentCulture.Name}); 
            }

            ErrorMessage = "Der skete en fejl, prøv igen";
            return new RedirectToPageResult("");
        }
        
        /**
         * gets the current logged in user
         */
        private string GetLoggedInUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
        }


    }
}
