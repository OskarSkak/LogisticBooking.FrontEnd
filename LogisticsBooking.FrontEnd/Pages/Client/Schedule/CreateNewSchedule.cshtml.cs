using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    [BindProperties]
    public class CreateNewSchedule : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly IMapper _mapper;

        
        [TempData]
        public string CompleteMessage { get; set; }
        
        [TempData]
        public string Message { get; set; }
        public List<InternalInterval> Intervals { get; set; }
        
        private IScheduleDataService ScheduleDataService { get;}
        private readonly string DAY = "DAY";
        private readonly string NIGHT = "NIGHT";
        
        [BindProperty]
        public Shift Shifts { get; set; }
        
        [BindProperty] public bool IsDay { get; set; }


        public void OnGet(string id)
        {
            
            if (id.Equals(DAY)) IsDay = true;
            if (id.Equals(NIGHT)) IsDay = false;
            
            Shifts = IsDay ? Shift.Day : Shift.Night;

            Intervals = PopulateList(Intervals);
        }

        public void OnPost()
        {
        }
        
        public CreateNewSchedule(IScheduleDataService scheduleDataService , IMasterScheduleDataService masterScheduleDataService , IMapper mapper)
        {
            _masterScheduleDataService = masterScheduleDataService;
            _mapper = mapper;
            ScheduleDataService = scheduleDataService;
        }

        public async Task<IActionResult> OnPostStandard(List<InternalInterval> intervals, string name , Shift shift)
        {
            var schedule = CreateScheduleFromInternalIntervals(intervals);
            schedule.Name = name;
            //var result = await ScheduleDataService.CreateSchedule(schedule);

            
            var masterScheduleViewModel = new MasterScheduleStandardViewModel
            {
                Name = name,
                Shifts = shift,
                CreatedBy = GetLoggedInUserId(),
                IsActive = false,
                MischellaneousPallets = schedule.MischellaneousPallets,
                MasterScheduleStandardId = Guid.NewGuid(),
                MasterIntervalStandardViewModels = _mapper.Map<List<MasterIntervalStandardViewModel>>(schedule.Intervals)
                
            };
            
            
            var result = await _masterScheduleDataService.CreateNewMasterSchedule(_mapper.Map<CreateNewMasterScheduleStandardCommand>(masterScheduleViewModel));

            if (result.IsSuccesfull)
            {
                CompleteMessage = "Planen er oprettet korrekt";
                return new RedirectToPageResult("ScheduleOverview");
            }


            Message = "Planen blev ikke oprettet korrekt, prøv igen";
            return new RedirectToPageResult("ScheduleOverview");

        }
        
        private bool CorrectDay(DateTime? dateTime, IntervalViewModel interval)
        {
            
            if (1 <= interval.EndTime.Value.Hour && interval.EndTime.Value.Hour < 22)
            {
                return true;
            } 
            
            

            return false;
        }

        public IActionResult OnPostSpecific(List<InternalInterval> intervals, string name , Shift shift)
        {
            var schedule = CreateScheduleFromInternalIntervals(intervals);
            schedule.Name = name;
            schedule.Shifts = shift;
            //var result = await ScheduleDataService.CreateSchedule(schedule);

            HttpContext.Session.SetObject("scheduleId", schedule);
            
            return new RedirectToPageResult("Calendar");
        }

        private ScheduleViewModel CreateScheduleFromInternalIntervals(List<InternalInterval> intervals)
        {
            var Schedule = new ScheduleViewModel();
            Schedule.Intervals = new List<IntervalViewModel>();

            foreach (var internalInterval in intervals)
            {
                if (internalInterval.BottomPallets != 0 && internalInterval.StartTime != DateTime.MinValue
                    && internalInterval.EndTime != DateTime.MinValue)
                {
                    var interval = new IntervalViewModel
                    {
                        BottomPallets = internalInterval.BottomPallets, 
                        StartTime = internalInterval.StartTime, 
                        EndTime = internalInterval.EndTime,
                        RemainingPallets = internalInterval.BottomPallets,
                        IntervalId = Guid.NewGuid() 
                    };
                    
                    Schedule.Intervals.Add(interval);
                }
            }
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            Schedule.CreatedBy = Guid.Parse(id);

            return Schedule;
        }

        private List<InternalInterval> PopulateList(List<InternalInterval> Intervals)
        {
            Intervals = new List<InternalInterval>();
            DateTime Time = new DateTime();
            if (IsDay)
            {
                Time = Convert.ToDateTime("07:00");
            }
            else
            {
                Time = Convert.ToDateTime("22:00");
            }
            
            for (int i = 0; i < 11; i++)
            {
                var interval = new InternalInterval();
                interval.StartTime = Time.AddMinutes(i * 45);
                interval.EndTime = Time.AddMinutes((i * 45) + 45);
                interval.BottomPallets = 33;
                Intervals.Add(interval);
                interval.InternalId = i;
                interval.IsShown = true;
            }

            for (int i = 11; i < 27; i++)
            {
                var interval = new InternalInterval();
                interval.InternalId = i;
                Intervals.Add(interval);
            }

            return Intervals;
        }
        
        private Guid GetLoggedInUserId()
        {
            var id =  User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            return Guid.Parse(id);
        }

        //DAY: 8 timer per default 7-15
        //Night: 8 timer 22-06
    }

    public class InternalInterval : IHaveCustomMapping
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int InternalId { get; set; }
        public int BottomPallets { get; set; }
        public bool IsShown { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<InternalInterval, MasterIntervalStandardViewModel>()
                .ForMember(dest => dest.BottomPallets,
                    opt => opt.MapFrom(src => src.BottomPallets))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.StartTime))
                ;
        }
    }
}
