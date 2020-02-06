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
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var currentBooking = TempData.Get<BookingViewModel>(id);
            
            // Get the schedule that match the booking. (Chech has already been made at the first page)
            var result = await _scheduleDataService.GetScheduleBydate(currentBooking.BookingTime);
            
            // remove the intervals that does not overlap with the suppliers time range. 
            // It is only possible to book a time with that match the selected suppliers on the orders. 

            if (result == null)
            {
                return await CreateScheduleFromActiveMaster(currentBooking.BookingTime , currentBooking);
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

        private async Task<IActionResult> CreateScheduleFromActiveMaster(DateTime bookingTime,
            BookingViewModel currentBooking)
        {
            var result = await _masterScheduleDataService.GetActiveMasterSchedule();
            if (result == null)
            {
                ErrorMessage = "Der er ingen aktiv plan på denne dag.";
            }

            var many = new CreateManyScheduleCommand();
            many.SchedulesListViewModel = new SchedulesListViewModel();
            many.SchedulesListViewModel.Schedules = new List<ScheduleViewModel>();


            var orderOnBooking = currentBooking.OrdersListViewModel.FirstOrDefault();

            var shift = Shift.Day;

            foreach (var masterSchedule in result.MasterScheduleStandardViewModels)
            {
                foreach (var interval in masterSchedule.MasterIntervalStandardViewModels)
                {
                    if ((orderOnBooking.SupplierViewModel.DeliveryStart.TimeOfDay >
                         interval.StartTime.Value.TimeOfDay) &&
                        (orderOnBooking.SupplierViewModel.DeliveryStart.TimeOfDay < interval.EndTime.Value.TimeOfDay))
                    {
                        shift = masterSchedule.Shifts;
                    }
                }
            }

            var masterScheduleStandardViewModel = result.MasterScheduleStandardViewModels.FirstOrDefault(e => e.Shifts == shift);

            if (masterScheduleStandardViewModel == null)
            {
                
            } 
            
            var scheduleId = Guid.NewGuid();

           
                many.SchedulesListViewModel.Schedules.Add(new ScheduleViewModel
                {
                    Name = masterScheduleStandardViewModel.Name,
                    Shifts = masterScheduleStandardViewModel.Shifts,
                    CreatedBy = masterScheduleStandardViewModel.CreatedBy,
                    MischellaneousPallets = masterScheduleStandardViewModel.MischellaneousPallets,
                    ScheduleDay = bookingTime,
                    Intervals = Map(masterScheduleStandardViewModel, scheduleId , currentBooking),
                    ScheduleId = scheduleId,
                    ActiveDays = _mapper.Map<List<DayViewModel>>(masterScheduleStandardViewModel.ActiveDays)
                }); 
            
            
            
                scheduleId = Guid.NewGuid();
                masterScheduleStandardViewModel = result.MasterScheduleStandardViewModels.FirstOrDefault(e => e.Shifts == Shift.Night);
                if (masterScheduleStandardViewModel != null)
                {
                    many.SchedulesListViewModel.Schedules.Add(new ScheduleViewModel
                    {
                        Name = masterScheduleStandardViewModel.Name,
                        Shifts = masterScheduleStandardViewModel.Shifts,
                        CreatedBy = masterScheduleStandardViewModel.CreatedBy,
                        MischellaneousPallets = masterScheduleStandardViewModel.MischellaneousPallets,
                        ScheduleDay = bookingTime,
                        Intervals = Fx(masterScheduleStandardViewModel.MasterIntervalStandardViewModels , currentBooking.BookingTime , scheduleId),
                        ScheduleId = scheduleId,
                        ActiveDays = _mapper.Map<List<DayViewModel>>(masterScheduleStandardViewModel.ActiveDays)
                    }); 
                }
                
            await _scheduleDataService.CreateManySchedule(many);
            
            // Return new redirect to same page in order to get the newlt created schedule
            return new RedirectToPageResult("");

        }

        public List<IntervalViewModel> Map(MasterScheduleStandardViewModel masterSchedulesStandardViewModel , Guid scheduleId , BookingViewModel bookingViewModel)
        {
            List<IntervalViewModel> intervals = new List<IntervalViewModel>();

            foreach (var interval in masterSchedulesStandardViewModel.MasterIntervalStandardViewModels)
            {
                intervals.Add(new IntervalViewModel
                {
                    Bookings = new List<BookingViewModel>(),
                    BottomPallets = interval.BottomPallets,
                    EndTime = new DateTime(bookingViewModel.BookingTime.Year , bookingViewModel.BookingTime.Month , bookingViewModel.BookingTime.Day , interval.EndTime.Value.Hour, interval.EndTime.Value.Minute , interval.EndTime.Value.Second),
                    StartTime =  new DateTime(bookingViewModel.BookingTime.Year , bookingViewModel.BookingTime.Month , bookingViewModel.BookingTime.Day , interval.StartTime.Value.Hour, interval.StartTime.Value.Minute , interval.StartTime.Value.Second),
                    IntervalId = Guid.NewGuid(),
                    IsBooked = false,
                    RemainingPallets = interval.BottomPallets,
                    ScheduleId = scheduleId
                });
            }

            return intervals;
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
                    EndTime = interval.EndTime,
                    StartTime = interval.StartTime,
                    
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
            var currentLoggedInUserId = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var currentBooking = TempData.Get<BookingViewModel>(currentLoggedInUserId);

            var createBookingcommand = _mapper.Map<CreateBookingCommand>(currentBooking);
            createBookingcommand.IntervalId = Guid.Parse(interval);
            createBookingcommand.TransporterId = currentBooking.TransporterId;

            var result = await _bookingDataService.CreateBooking(createBookingcommand);

            if (result.IsSuccesfull)
            {
                return RedirectToPage("confirm"); 
            }

            ErrorMessage = "Der skete en fejl, prøv igen";
            return new RedirectToPageResult("");
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
