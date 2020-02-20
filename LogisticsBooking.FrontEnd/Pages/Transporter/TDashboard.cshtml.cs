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
    public class TDashboard : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly ILogger<DashboardViewModel> _logger;
        private readonly IMapper _mapper;
        private readonly IDashboardDataService _dashboardDataService;


        [BindProperty]
        public MasterSchedulesStandardViewModel MasterSchedulesStandardViewModel { get; set; }

        [BindProperty]
        public TransporterDashboardViewModel TransporterDashboardViewModel { get; set; }
        
        [TempData]
        public String Message { get; set; }
        
        public bool ShowResponseMessage => !String.IsNullOrEmpty(Message);
        
        [BindProperty]
        public int ShowPercent { get; set; }

        public TDashboard(IDashboardDataService dashboardDataService , IMasterScheduleDataService masterScheduleDataService , ILogger<DashboardViewModel> logger)
        {
            _dashboardDataService = dashboardDataService;
            _masterScheduleDataService = masterScheduleDataService;
            _logger = logger;
        }
        
        public async Task<IActionResult> OnGet()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;
            TransporterDashboardViewModel = await _dashboardDataService.GetTransporterDashboard(Guid.Parse(id));
            
            return Page();
        }
        

      
        public void OnPostDateTime()
        {
            throw new NullReferenceException();


        }
        
        
        
       

        
    }
}