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
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>(currentLoggedInUserId);

            // Get the schedule that match the booking. (Chech has already been made at the first page)
            var result = await _scheduleDataService.GetScheduleBydate(currentBooking.BookingTime);
            
            // remove the intervals that does not overlap with the suppliers time range. 
            // It is only possible to book a time with that match the selected suppliers on the orders. 

            if (result == null)
            {
                return await CreateScheduleFromActiveMaster(currentBooking.BookingTime);
            }

            foreach (var interval in result.Schedules)
            {
                 RemoveIntervalsNotOverlap(currentBooking , interval);
            }
                
            // Set the Schedule to be shown in the view and sort it. 
            SchedulesListViewModel = result;
            foreach (var sched in SchedulesListViewModel.Schedules)
            {
                sched.Intervals = sched.Intervals.OrderBy(e => e.StartTime.Value).ToList();
            }
            
            return Page();
            
        }

        private async Task<IActionResult> CreateScheduleFromActiveMaster(DateTime bookingTime)
        {
            var result = await _masterScheduleDataService.GetActiveMasterSchedule();
            if (result == null)
            {
                // Ingen aktive Schedules
                return new RedirectToPageResult("Error");
            }
            var many = new CreateManyScheduleCommand();
            many.SchedulesListViewModel = new SchedulesListViewModel();
            many.SchedulesListViewModel.Schedules = new List<ScheduleViewModel>();

            
            foreach (var masterSchedule in result.MasterScheduleStandardViewModels)
            {
                var scheduleId = Guid.NewGuid();
                many.SchedulesListViewModel.Schedules.Add(new ScheduleViewModel
                {
                    Name = masterSchedule.Name,
                    Shifts = masterSchedule.Shifts,
                    CreatedBy = masterSchedule.CreatedBy,
                    MischellaneousPallets = masterSchedule.MischellaneousPallets,
                    ScheduleDay = bookingTime,
                    Intervals = Fx(masterSchedule.MasterIntervalStandardViewModels, bookingTime , scheduleId),
                    ScheduleId = scheduleId
                });

            }
            


            await _scheduleDataService.CreateManySchedule(many);
            return new RedirectToPageResult("");

        }


        private List<IntervalViewModel> Fx(List<MasterIntervalStandardViewModel> masterIntervalStandardViewModels , DateTime bookingTime , Guid scheduleId)
        {
           
            var list = new List<MasterIntervalStandardViewModel>();
            foreach (var interval in masterIntervalStandardViewModels)
            {
                var masterInterval = new MasterIntervalStandardViewModel
                {
                    BottomPallets = interval.BottomPallets,
                    MasterIntervalStandardId = Guid.NewGuid(),
                    MasterScheduleStandardId = scheduleId,
                };
                
                if (interval.StartTime.Value.Hour >22 && interval.EndTime.Value.Hour < 10 )
                { 
                    
                    masterInterval.EndTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day+1 , interval.EndTime.Value.Hour , interval.EndTime.Value.Minute , interval.EndTime.Value.Second);
                    masterInterval.StartTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day , interval.StartTime.Value.Hour , interval.StartTime.Value.Minute , interval.StartTime.Value.Second);
                }
                else if (interval.StartTime.Value.Hour < 10 )
                {
                    masterInterval.EndTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day+1 , interval.EndTime.Value.Hour , interval.EndTime.Value.Minute , interval.EndTime.Value.Second);
                    masterInterval.StartTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day+1 , interval.StartTime.Value.Hour , interval.StartTime.Value.Minute , interval.StartTime.Value.Second);
                }
                else
                {
                    masterInterval.EndTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day , interval.EndTime.Value.Hour , interval.EndTime.Value.Minute , interval.EndTime.Value.Second);
                    masterInterval.StartTime = new DateTime(bookingTime.Year , bookingTime.Month , bookingTime.Day , interval.StartTime.Value.Hour , interval.StartTime.Value.Minute , interval.StartTime.Value.Second);
                }
                
                list.Add(masterInterval);
            }

            return _mapper.Map<List<IntervalViewModel>>(list);
        }
        
        public async Task<IActionResult> OnPostSelectedTime(string interval , ScheduleViewModel schedule)
        {
            var currentLoggedInUserId = GetLoggedInUserId();

            // Get the current booking View Model from the session created at previous page
            var currentBooking = HttpContext.Session.GetObject<BookingViewModel>(currentLoggedInUserId);



            var createBookingcommand = _mapper.Map<CreateBookingCommand>(currentBooking);
            createBookingcommand.IntervalId = Guid.Parse(interval);
            createBookingcommand.TransporterId = Guid.Parse(currentLoggedInUserId);

            var result = await _bookingDataService.CreateBooking(createBookingcommand);

            if (result.IsSuccesfull)
            {
                return RedirectToPage("confirm"); 
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
        
        private bool Overlap(IntervalViewModel intervalViewModel,
            OrderViewModel orderViewModel)
        {
            
            TimeSpan start = orderViewModel.SupplierViewModel.DeliveryStart.TimeOfDay; // 10 PM
            TimeSpan end = orderViewModel.SupplierViewModel.DeliveryEnd.TimeOfDay;   // 2 AM
            TimeSpan start1 = intervalViewModel.StartTime.Value.TimeOfDay;
            TimeSpan end1 = intervalViewModel.EndTime.Value.TimeOfDay;
            return TimeUtility.IsWithin(start, end, start1, end1);
            
        }


        private void RemoveIntervalsNotOverlap(BookingViewModel bookingViewModel, ScheduleViewModel scheduleViewModel)
        {
            foreach (var order in bookingViewModel.OrdersListViewModel)
            {
                scheduleViewModel.Intervals.RemoveAll(item => scheduleViewModel.Intervals.Any( iss => !Overlap(item ,order )));
            }


        }

       

    }
}
