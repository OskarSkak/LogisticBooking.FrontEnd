using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class UpdateScheduleMaster : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly IScheduleDataService _scheduleDataService;
        private readonly IMapper _mapper;


        [BindProperty] public MasterScheduleStandardViewModel MasterScheduleStandardViewModel { get; set; }

        [BindProperty]
        public List<NewIntervalViewModel> NewIntervalViewModels { get; set; } = new List<NewIntervalViewModel>();



        [BindProperty] public DateTime NewScheduleDate { get; set; }

        [BindProperty] public Guid MasterScheduleId { get; set; }
        [BindProperty] public Guid CreatedBy { get; set; }

        public UpdateScheduleMaster(IMasterScheduleDataService masterScheduleDataService , IScheduleDataService scheduleDataService , IMapper mapper)
        {
            _masterScheduleDataService = masterScheduleDataService;
            _scheduleDataService = scheduleDataService;
            _mapper = mapper;

            for (int i = 0; i < 10; i++)
            {
                NewIntervalViewModels.Add(new NewIntervalViewModel());
            }

        }


        public async Task OnGet(Guid id, DateTime date , Guid createdBy)
        {
            MasterScheduleStandardViewModel = await _masterScheduleDataService.GetMasterScheduleById(id);
            NewScheduleDate = date;
            MasterScheduleId = id;
            CreatedBy = createdBy;
        }

        public async Task<IActionResult> OnPostUpdate(List<NewIntervalViewModel> NewIntervals,
            List<MasterIntervalStandardViewModel> intervals, List<string> day, string nameOfPlan, Shift shift,
            DateTime NewScheduleDate, Guid MasterScheduleId , Guid CreatedBy)
        {
            var createCommand = new CreateScheduleFromMasterCommand();

            createCommand.Name = nameOfPlan;
            createCommand.Shifts = shift;
            createCommand.IsStandard = false;
            createCommand.ScheduleDay = NewScheduleDate;
            createCommand.MischellaneousPallets = 0;
            createCommand.CreatedBy = CreatedBy;

            var scheduleId = Guid.NewGuid();
            createCommand.ScheduleId = scheduleId;

            createCommand.ActiveDays = SetDayViewModel(day);
            
            var intervall = new List<IntervalViewModel>();
            foreach (var interval in intervals)
            {
                if (interval.EndTime.Value.TimeOfDay != TimeSpan.Zero ||
                    interval.StartTime.Value.TimeOfDay != TimeSpan.Zero)
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

            foreach (var newInterval in NewIntervals)
            {
                if (newInterval.End != TimeSpan.Zero ||
                    newInterval.Start != TimeSpan.Zero)
                {
                    intervall.Add(new IntervalViewModel
                    {
                        Bookings = new List<BookingViewModel>(),
                        BottomPallets = newInterval.Pallets,
                        EndTime = CreateDate(NewScheduleDate , newInterval.End),
                        StartTime = CreateDate(NewScheduleDate , newInterval.Start),
                        IntervalId = Guid.NewGuid(),
                        IsBooked = false,
                        RemainingPallets = newInterval.Pallets,
                        ScheduleId = scheduleId
                    });
                }
            }

            List<IntervalViewModel> sorted = intervall.OrderBy(e => e.StartTime).ToList();

            createCommand.IntervalViewModels = sorted;


            var result = await _scheduleDataService.CreateScheduleFromMAster(createCommand);

            if (!result.IsSuccesfull)
            {
                //TODO Skriv det ikke lykkedes
            }

            return new RedirectToPageResult("CalendarOverview");
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
    
    public class NewIntervalViewModel
    {
        public TimeSpan Start { get; set; }
            
        public TimeSpan End { get; set; }
            
        public int Pallets { get; set; }

    }

}