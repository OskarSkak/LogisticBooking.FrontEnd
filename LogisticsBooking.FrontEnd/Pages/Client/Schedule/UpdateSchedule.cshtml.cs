using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class UpdateSchedule : PageModel
    {
        private readonly IScheduleDataService _scheduleDataService;
        private readonly IBookingDataService _bookingDataService;
        [BindProperty] public DateTime NewScheduleDate { get; set; }

        [BindProperty] public Guid CurrentScheduleId { get; set; }
        [BindProperty] public Guid CreatedBy { get; set; }
        
        
        [BindProperty]
        public List<NewIntervalViewModel> NewIntervalViewModels { get; set; } = new List<NewIntervalViewModel>();
        
        [BindProperty]
        public ScheduleViewModel ScheduleViewModel { get; set; }


        public UpdateSchedule(IScheduleDataService scheduleDataService , IBookingDataService bookingDataService)
        {
            _scheduleDataService = scheduleDataService;
            _bookingDataService = bookingDataService;
        }
        
        public async Task OnGet(Guid id, DateTime date , Guid createdBy)
        {
            ScheduleViewModel = await _scheduleDataService.GetScheduleById(id);
            
            for (int i = 0; i < 10; i++)
            {
                NewIntervalViewModels.Add(new NewIntervalViewModel());
            }

            ScheduleViewModel.Intervals = ScheduleViewModel.Intervals.OrderBy(e => e.StartTime).ToList();
            NewScheduleDate = date;
            CreatedBy = createdBy;
            CurrentScheduleId = id;
        }

        public async Task<IActionResult> OnPostUpdate(List<NewIntervalViewModel> NewIntervals,
            List<IntervalViewModel> intervals, List<string> day, string nameOfPlan, Shift shift,
            DateTime NewScheduleDate, Guid CurrentScheduleId, Guid CreatedBy)
        {

            var currentSchedule = await _scheduleDataService.GetScheduleById(CurrentScheduleId);
            
            
            
            List<BookingViewModel> NotToBeDeleted = new List<BookingViewModel>();
            var createCommand = new CreateScheduleFromActiveSchedule();
          
            createCommand.Name = nameOfPlan;
            createCommand.Shifts = shift;
            createCommand.IsStandard = false;
            createCommand.ScheduleDay = NewScheduleDate;
            createCommand.MischellaneousPallets = 0;
            createCommand.CreatedBy = CreatedBy;

            var scheduleId = currentSchedule.ScheduleId;
            createCommand.ScheduleId = scheduleId;

            createCommand.ActiveDays = SetDayViewModel(day);
            
            var intervall = new List<IntervalViewModel>();
            foreach (var interval in intervals)
            {
                if (interval.EndTime.Value.TimeOfDay != TimeSpan.Zero ||
                    interval.StartTime.Value.TimeOfDay != TimeSpan.Zero)
                {

                    var sameInterval = currentSchedule.Intervals.FirstOrDefault(e =>
                        e.EndTime.Value.TimeOfDay.CompareTo(interval.EndTime.Value.TimeOfDay) == 0 &&
                        e.StartTime.Value.TimeOfDay.CompareTo(interval.StartTime.Value.TimeOfDay) == 0);

                    if (sameInterval != null)
                    {
                        intervall.Add(sameInterval);
                        NotToBeDeleted.AddRange(sameInterval.Bookings);
                    }
                    else
                    {
                        intervall.Add(new IntervalViewModel
                        {
                            Bookings = new List<BookingViewModel>(),
                            BottomPallets = interval.BottomPallets,
                            EndTime = CreateDate(NewScheduleDate , interval.EndTime.Value.TimeOfDay),
                            StartTime = CreateDate(NewScheduleDate , interval.StartTime.Value.TimeOfDay),
                            IntervalId = Guid.NewGuid(),
                            IsBooked = false,
                            RemainingPallets = interval.BottomPallets,
                            ScheduleId = scheduleId
                        });  
                    }
                    
                    
                    
                    
                }
            }

            foreach (var newInterval in NewIntervals)
            {
                if (newInterval.End != TimeSpan.Zero ||
                    newInterval.Start != TimeSpan.Zero)
                {
                   
                    var sameInterval = currentSchedule.Intervals.FirstOrDefault(e =>
                        e.EndTime.Value.TimeOfDay.CompareTo(newInterval.End) == 0 &&
                        e.StartTime.Value.TimeOfDay.CompareTo(newInterval.Start) == 0);

                    if (sameInterval != null)
                    {
                        intervall.Add(sameInterval);
                        NotToBeDeleted.AddRange(sameInterval.Bookings);
                    }
                    else
                    {
                        intervall.Add(new IntervalViewModel
                        {
                            Bookings = new List<BookingViewModel>(),
                            BottomPallets = newInterval.Pallets,
                            EndTime = CreateDate(NewScheduleDate , newInterval.End),
                            StartTime = CreateDate(NewScheduleDate , newInterval.End),
                            IntervalId = Guid.NewGuid(),
                            IsBooked = false,
                            RemainingPallets = newInterval.Pallets,
                            ScheduleId = scheduleId
                        });  
                    }
                }
            }

            

            List<BookingViewModel> bookingToBeDeleted = new List<BookingViewModel>();

            foreach (var interval in currentSchedule.Intervals)
            {
                foreach (var booking in interval.Bookings)
                {
                    var notDeleted = NotToBeDeleted.FirstOrDefault(e => e.InternalId == booking.InternalId);
                    if (notDeleted == null)
                    {
                        bookingToBeDeleted.Add(booking);
                    }
                }
            }

            foreach (var booking in bookingToBeDeleted)
            {
                var bookingResult = await _bookingDataService.DeleteBooking(booking.InternalId);
                Console.WriteLine(bookingResult);
            }
            
            List<IntervalViewModel> sorted = intervall.OrderBy(e => e.StartTime).ToList();

            createCommand.IntervalViewModels = sorted;
            var result = await _scheduleDataService.CreateScheduleFromSchedule(createCommand);

            if (result.IsSuccesfull)
            {
                return new RedirectToPageResult("CalendarOverview" , new {culture = CultureInfo.CurrentCulture.Name});
            }

            return new RedirectToPageResult("CalendarOverview" , new {culture = CultureInfo.CurrentCulture.Name});
        }



        public async Task<IActionResult> OnPostDelete(Guid CurrentScheduleId)
        {
            var currentSchedule = await _scheduleDataService.GetScheduleById(CurrentScheduleId);

            foreach (var interval in currentSchedule.Intervals)
            {
                foreach (var booking in interval.Bookings)
                {
                   await  _bookingDataService.DeleteBooking(booking.InternalId);
                }
            }

            var result = await _scheduleDataService.DeleteSchedule(currentSchedule.ScheduleId);

            if (result.IsSuccesfull)
            {
                return new RedirectToPageResult("CalendarOverview" , new {culture = CultureInfo.CurrentCulture.Name});
            }
            
            return new RedirectToPageResult("CalendarOverview" , new  {culture = CultureInfo.CurrentCulture.Name});
        }
        
        private DateTime CreateDate(DateTime datetime, TimeSpan time )
        {
            return new DateTime(datetime.Year , datetime.Month , datetime.Day , time.Hours , time.Minutes , time.Seconds);
        }
        
         private List<DayViewModel> SetDayViewModel(List<string> days)
        {
            List<DayViewModel> MasteractiveDaysViewModels = new List<DayViewModel>();
            
            
            foreach (var day in days)
            {
                if (day == "monday")
                {
                    MasteractiveDaysViewModels.Add(new DayViewModel
                    {
                        ActiveDay = DayOfWeek.Monday,
                        IsActive = true,
                    });
                }
                if (day == "tuesday")
                {
                    MasteractiveDaysViewModels.Add(new DayViewModel
                    {
                        ActiveDay = DayOfWeek.Tuesday,
                        IsActive = true,
                    });
                }
                if (day == "wednesday")
                {
                    MasteractiveDaysViewModels.Add(new DayViewModel
                    {
                        ActiveDay = DayOfWeek.Wednesday,
                        IsActive = true,
                    });
                }
                if (day == "thursday")
                {
                    MasteractiveDaysViewModels.Add(new DayViewModel
                    {
                        ActiveDay = DayOfWeek.Thursday,
                        IsActive = true,
                    });
                }
                if (day == "friday")
                {
                    MasteractiveDaysViewModels.Add(new DayViewModel
                    {
                        ActiveDay = DayOfWeek.Friday,
                        IsActive = true,
                    });
                }
                
                if (day == "saturday")
                {
                    MasteractiveDaysViewModels.Add(new DayViewModel
                    {
                        ActiveDay = DayOfWeek.Saturday,
                        IsActive = true,
                    });
                }
                
                
                if (day == "sunday")
                {
                    MasteractiveDaysViewModels.Add(new DayViewModel
                    {
                        ActiveDay = DayOfWeek.Sunday,
                        IsActive = true,
                    });
                }
               
                
            }

            

            return MasteractiveDaysViewModels;
        }
    }
}