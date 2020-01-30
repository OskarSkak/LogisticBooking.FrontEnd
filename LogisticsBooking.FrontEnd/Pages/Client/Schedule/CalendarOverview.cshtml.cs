using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class CalendarOverview : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly IScheduleDataService _scheduleDataService;

        
        
        public SchedulesListViewModel SchedulesListViewModel { get; set; }
        public CalenderViewModel CalenderViewModel { get; set; }
        
        [TempData]
        public String Message { get; set; }
        
        public MasterSchedulesStandardViewModel MasterSchedulesStandardViewModel { get; set; }
        
        public CalendarOverview(IScheduleDataService scheduleDataService , IMapper mapper , IMasterScheduleDataService masterScheduleDataService)
        {
     
            _scheduleDataService = scheduleDataService;
            _mapper = mapper;
            _masterScheduleDataService = masterScheduleDataService;
        }
        
        
        public async Task OnGet()
        {



            SchedulesListViewModel = await _scheduleDataService.GetSchedules();
            
            var calender =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calender == null)
            {
                calender = new CalenderViewModel();
            }
            
            CalenderViewModel = calender;

            foreach (var schedule in SchedulesListViewModel.Schedules)
            {
              schedule.Intervals =  schedule.Intervals.OrderBy(x => x.StartTime).ToList();
            }

            MasterSchedulesStandardViewModel = await _masterScheduleDataService.GetActiveMasterSchedule();

        }
        
        [ValidateAntiForgeryToken]
        [EnableCors("MyPolicy")]
        public IActionResult OnPostForward([FromBody]string[] value)
        { 
            var calendar =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calendar == null)
            {
                calendar = new CalenderViewModel();
            }


           
           calendar.AdvanceMonth();
           HttpContext.Session.SetObject("key" , calendar);

           

           return new RedirectToPageResult("");
        }
    

        public void getDate()
        {
            
        }

        [ValidateAntiForgeryToken]
        [EnableCors("MyPolicy")]
        public IActionResult OnPostBack([FromBody]string[] value)
        {
            
            var calendar =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calendar == null)
            {
                calendar = new CalenderViewModel();
            }
            
            calendar.DecreaseMonth();
            HttpContext.Session.SetObject("key" , calendar);

            return new RedirectToPageResult("");
           

           
            
        }

        public async Task<IActionResult> OnPostDeleteSchedule(Guid id)
        {

            var response = await _scheduleDataService.DeleteSchedule(id);

            if (response.IsSuccesfull)
            {
                Message = "Planen er slettet korrekt";
            }
            
            
            
            return new RedirectToPageResult("");
        }

        
        
    }
}