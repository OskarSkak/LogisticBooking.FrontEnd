using System;
using System.Collections.Generic;
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
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class select_timeAdmin : PageModel
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
        
        [TempData]
        public string Message { get; set; }

        public bool ShowErrorMessage => !String.IsNullOrEmpty(ErrorMessage);

        public select_timeAdmin(IBookingDataService bookingDataService , IScheduleDataService scheduleDataService , IMapper mapper , IMasterScheduleDataService masterScheduleDataService)
        {
            _bookingDataService = bookingDataService;
            _scheduleDataService = scheduleDataService;
            _mapper = mapper;
            _masterScheduleDataService = masterScheduleDataService;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>(id);
            
            // Get the schedule that match the booking. (Chech has already been made at the first page)
            
            
            // remove the intervals that does not overlap with the suppliers time range. 
            // It is only possible to book a time with that match the selected suppliers on the orders. 

            SchedulesListViewModel =  await _scheduleDataService.GenerateScheduleFromCurrentBooking(new GenerateScheduleForNewBookingCommand{BookingTime = currentBooking.BookingTime, SupplierViewModels = currentBooking.SuppliersListViewModel});

            

            return Page();
            
        }

        public async Task<IActionResult> OnPostSelectedTime(string interval , ScheduleViewModel schedule)
        {
            var currentLoggedInUserId = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>(currentLoggedInUserId);

            var createBookingcommand = _mapper.Map<CreateBookingCommand>(currentBooking);
            createBookingcommand.IntervalId = Guid.Parse(interval);
            createBookingcommand.TransporterId = currentBooking.TransporterId;
            createBookingcommand.IsValidated = true;
            var result = await _bookingDataService.CreateBooking(createBookingcommand);

            if (result.IsSuccesfull)
            {
                Message = "Bookingen er oprettet";
                return new RedirectToPageResult("/Client/Dashboard"); 
            }

            ErrorMessage = "Der skete en fejl, prøv igen";
            return new RedirectToPageResult("");
            

            
        }
        
    
       
    }
}
