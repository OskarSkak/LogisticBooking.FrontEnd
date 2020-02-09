using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Dashboard;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;
using Serilog.Context;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    public class Dashboard : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly ILogger<DashboardViewModel> _logger;
        private readonly IMapper _mapper;
        private readonly IDashboardDataService _dashboardDataService;


        [BindProperty]
        public MasterSchedulesStandardViewModel MasterSchedulesStandardViewModel { get; set; }

        [BindProperty]
        public DashboardViewModel DashboardViewModel { get; set; }
        
        [BindProperty]
        public int ShowPercent { get; set; }

        public Dashboard(IDashboardDataService dashboardDataService , IMasterScheduleDataService masterScheduleDataService , ILogger<DashboardViewModel> logger)
        {
            _dashboardDataService = dashboardDataService;
            _masterScheduleDataService = masterScheduleDataService;
            _logger = logger;
        }
        
        public async Task<IActionResult> OnGet()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = await _masterScheduleDataService.GetActiveMasterSchedule();
            

            MasterSchedulesStandardViewModel = CreateMasterSchedules(result);

            DashboardViewModel = await _dashboardDataService.GetDashboard();

            var day = new TimeSpan(24, 0, 0);
            var percent =  (double) DashboardViewModel.TimeToNextDelivery.Ticks / (double) day.Ticks ;
            var d = 1 - percent;
            ShowPercent = (int) (d * 100);
            
            stopwatch.Stop();
            using (LogContext.PushProperty("X-Correlation-ID", HttpContext.TraceIdentifier))
            {
                _logger.LogWarning("Dashboard OnGet Took {time}ms" , stopwatch.ElapsedMilliseconds);
            }

            return Page();
        }
        

        private MasterSchedulesStandardViewModel CreateMasterSchedules(MasterSchedulesStandardViewModel masterSchedulesStandardViewModel)
        {
            List<MasterIntervalStandardViewModel> DayIntervale = new List<MasterIntervalStandardViewModel>();
            List<MasterIntervalStandardViewModel> NightInterval = new List<MasterIntervalStandardViewModel>();

            foreach (var masterScheduleStandardView in masterSchedulesStandardViewModel.MasterScheduleStandardViewModels)
            {
                if (masterScheduleStandardView.Shifts == Shift.Day)
                {
                    DayIntervale =
                        masterScheduleStandardView.MasterIntervalStandardViewModels.OrderBy(e => e.StartTime).ToList();

                    masterScheduleStandardView.MasterIntervalStandardViewModels = DayIntervale;
                }
                else
                {
                    foreach (var interval in masterScheduleStandardView.MasterIntervalStandardViewModels)
                    {
                        if (interval.StartTime.Value.Hour < 16)
                        {
                            interval.StartTime = new DateTime(interval.StartTime.Value.Year , interval.StartTime.Value.Month , interval.StartTime.Value.Day+1, interval.StartTime.Value.Hour , interval.StartTime.Value.Minute, interval.StartTime.Value.Second);
                            
                            
                        }
                        
                    }
                    
                    NightInterval = masterScheduleStandardView.MasterIntervalStandardViewModels.OrderBy(c=> c.StartTime.Value.Date)
                        .ThenBy(c=> c.StartTime.Value.TimeOfDay).ToList();
                    
                    masterScheduleStandardView.MasterIntervalStandardViewModels = NightInterval;
                }

                
            }

            return masterSchedulesStandardViewModel;
        }

        public void OnPostDateTime()
        {
            throw new NullReferenceException();


        }
        
        
        
       

        
    }
}