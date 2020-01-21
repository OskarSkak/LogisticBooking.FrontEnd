using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{


    [ValidateAntiForgeryToken]
    public class Calendar : PageModel
    {
        private readonly IScheduleDataService _scheduleDataService;


        

        public SchedulesListViewModel SchedulesListViewModel { get; set; }


        [TempData]
        public String Message { get; set; }

        public DateChosen DateChosen { get; set; }

        public CalenderViewModel CalenderViewModel { get; set; }


       
     


        public Calendar(IScheduleDataService scheduleDataService)
        {
            _scheduleDataService = scheduleDataService;
        }
      
        public async Task<IActionResult> OnGet()
        {
            var Subjectid = "";
            
            
            Subjectid = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
            var calender =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calender == null)
            {
                calender = new CalenderViewModel();
            }

            var dates =  HttpContext.Session.GetObject<DateChosen>("datechosen");

            if (dates == null)
            {
                dates = new DateChosen();
                dates.CosenDays = new List<DateTime>();
            }

            DateChosen = dates;
            
            
            CalenderViewModel = calender;

            SchedulesListViewModel = await _scheduleDataService.GetSchedules();

            return Page();
            /*

            SchedulesListViewModel = await _scheduleDataService.GetSchedules();
            var test = HttpContext.Session.GetObject<DataServices.Models.Schedule>("v");
            var Schedule = HttpContext.Session.GetObject<DataServices.Models.Schedule>("scheduleId");
            
            
            var Subjectid = "";
            
           
            HttpContext.Session.SetObject("v" , Schedule);
            
            Subjectid = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var result = HttpContext.Session.GetObject<DateTime>(Subjectid);
            if (result == DateTime.MinValue)
            {
                result = date;
                result =  new DateTime(date.Year, date.Month, 1);
            }
            
            date = result;
            currentMonth = date.Month;
            CurrentYear = date.Year;
            Console.WriteLine(date);

            return Page();
            
            
            */

            
        }


        
        public async Task<IActionResult> OnPostConfirm([FromBody] string[] value)
        {
            
            var calendar =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calendar == null)
            {
                calendar = new CalenderViewModel();
            }
            var DateChosen =  HttpContext.Session.GetObject<DateChosen>("datechosen");

            if (DateChosen == null)
            {
                DateChosen = AddDatesToList(value , new DateChosen() , calendar);
            }
            
            DateChosen = AddDatesToList(value , DateChosen , calendar);

           
            DateChosen = CheckSameDates(DateChosen);

            foreach (var VARIABLE in DateChosen.CosenDays)
            {
                Console.WriteLine(VARIABLE.ToString("d"));
            }
            
           

            Message = "Planen er nu opretttet korrekt";
            
         
            /*
            List<DateTime> list = new List<DateTime>(); 
            var result = HttpContext.Session.GetObject<ScheduleViewModel>("v");
            
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != null)
                {
                    list.Add(DateTime.Parse(value[i]));
                    
                }
            }
*/
            var result = HttpContext.Session.GetObject<ScheduleViewModel>("scheduleId");

           
            
            SchedulesListViewModel schedules = new SchedulesListViewModel();
            foreach (var date in DateChosen.CosenDays)
            {
                var scheduleID = Guid.NewGuid();
                List<IntervalViewModel> intervals = new List<IntervalViewModel>();
                foreach (var interval in result.Intervals)
                {

                    DateTime starttime, endtime;	
                    	
                    if (CorrectDay(date, interval))	
                    {	
                        starttime = date.AddDays(1).Add(interval.StartTime.Value.TimeOfDay);	
                        endtime = date.AddDays(1).Add(interval.EndTime.Value.TimeOfDay);	
                    }	
                    else	
                    {	
                        starttime = date.AddDays(0).Add(interval.StartTime.Value.TimeOfDay);	
                        endtime = date.AddDays(0).Add(interval.EndTime.Value.TimeOfDay); 	
                    }
                    
                    intervals.Add(new IntervalViewModel
                    {
                        IntervalId = Guid.NewGuid(),
                        
                        BottomPallets = interval.BottomPallets,
                        EndTime = endtime,
                        StartTime = starttime,
                        RemainingPallets = interval.BottomPallets,
                        Bookings = new List<BookingViewModel>(),
                        IsBooked = false,
                        ScheduleId = scheduleID
                        
                        
                    });
                }
                
                var schedule = new ScheduleViewModel()
                {
                    CreatedBy = result.CreatedBy,
                    Intervals = intervals,
                    MischellaneousPallets = result.MischellaneousPallets,
                    Name = result.Name,
                    ScheduleDay = date,
                    ScheduleId = scheduleID,
                    Shifts = result.Shifts,
                    IsStandard = false
                };
                
                    schedules.Schedules.Add(schedule);
                

            }

            
            var vm = new SchedulesListViewModel();
            var vm1 = new CreateManyScheduleCommand();
            vm.Schedules = schedules.Schedules;
            vm1.SchedulesListViewModel = vm;
            var response = await _scheduleDataService.CreateManySchedule(vm1);

            if (response.IsSuccesfull)
            {
                Message = "Planen er nu oprettet Korrekt";
            }
            else
            {
                Message = response.HttpResponse.RequestMessage.ToString();
            }
            
            HttpContext.Session.Clear();
            return new ObjectResult(HttpStatusCode.OK);
            
            
            
            


        }

        private void testmethod(IntervalViewModel interval, in DateTime date)
        {
            if (interval.StartTime.Value.Hour < 24)
            {
                interval.StartTime = interval.StartTime.Value.AddDays(1);
            }
        }

        private bool CorrectDay(DateTime dateTime, IntervalViewModel interval)
        {
            
            if (1 <= interval.EndTime.Value.Hour && interval.EndTime.Value.Hour < 22)
            {
                return true;
            } 
            
            

            return false;
        }
        
        
        
        [EnableCors("MyPolicy")]
        public IActionResult OnPostForward([FromBody]string[] value)
        { 
            var calendar =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calendar == null)
            {
                calendar = new CalenderViewModel();
            }

          

           
            
           var DateChosen =  HttpContext.Session.GetObject<DateChosen>("datechosen");

           if (DateChosen == null)
           {
               DateChosen = AddDatesToList(value , new DateChosen() , calendar);
           }
           DateChosen = AddDatesToList(value , DateChosen , calendar);

           DateChosen = CheckSameDates(DateChosen);
           
           calendar.AdvanceMonth();
           HttpContext.Session.SetObject("key" , calendar);
            
           HttpContext.Session.SetObject("datechosen" ,  DateChosen);
           
           return new ObjectResult(HttpStatusCode.OK);
            /*
            var id = "";
            
           
                id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
                var result = HttpContext.Session.GetObject<DateTime>(id);
                if (result == DateTime.MinValue)
                {
                    result =  new DateTime(date.Year, date.Month, 1);
                    
                }
                else
                {
                    result = new DateTime(result.Year, result.Month, 1);
                    
                }

                currentMonth = (result.Month + 1) % 13;

                if (currentMonth == 0)
                {
                    currentMonth++;
                    CurrentYear = result.Year;
                    CurrentYear++;
                    result = new DateTime(CurrentYear, currentMonth, 1);
                }
                else
                {
                    result = new DateTime(result.Year, currentMonth, 1);
                }
                
                
                
                HttpContext.Session.SetObject(id , result);
                */
                return new RedirectToPageResult("Calendar");
        }
    

        public void getDate()
        {
            
        }

        
        [EnableCors("MyPolicy")]
    
        public IActionResult OnPostBack([FromBody]string[] value)
        {
            
            var calendar =  HttpContext.Session.GetObject<CalenderViewModel>("key");

            if (calendar == null)
            {
                calendar = new CalenderViewModel();
            }

            
            

           

            var DateChosen =  HttpContext.Session.GetObject<DateChosen>("datechosen");

            if (DateChosen == null)
            {
                
                DateChosen = AddDatesToList(value ,new DateChosen() , calendar );
            }

            DateChosen = AddDatesToList(value , DateChosen , calendar);

           
            DateChosen = CheckSameDates(DateChosen);
            
            calendar.DecreaseMonth();
            HttpContext.Session.SetObject("key" , calendar);
            HttpContext.Session.SetObject("datechosen" ,  DateChosen);
            
            
            
            return new ObjectResult(HttpStatusCode.OK);
            /*
            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var result = HttpContext.Session.GetObject<DateTime>(id);

            currentMonth = result.Month;
            CurrentYear = result.Year;
            
            if (result == DateTime.MinValue)
            {
                result =  new DateTime(date.Year, date.Month, 1);
                    
            }
            else
            {
                result = new DateTime(result.Year, result.Month, 1);
                    
            }
            if (currentMonth == 1)
            {
                currentMonth=12;
                CurrentYear-- ;
            }
            else
            {
                currentMonth--;
            }
            
            
            result = new DateTime(CurrentYear, currentMonth, 1);
            HttpContext.Session.SetObject( id , result);
            
            */
            
            DateTime myDT = new DateTime( 2002, 4, 3, new GregorianCalendar() );

           

            return new RedirectToPageResult("Calendar");

           
            
        }

        private DateChosen CheckSameDates(DateChosen dateChosen)
        {
            var dates = dateChosen.CosenDays.Select(d => d.Date).Distinct().ToList();

            dateChosen.CosenDays = dates;

            return dateChosen;
        }

        public ScheduleViewModel DateAlreadyHasSchedule(DateTime dateTime, SchedulesListViewModel schedules)
        {
            foreach (var schedule in SchedulesListViewModel.Schedules)
            {
                if (dateTime.Date == dateTime.Date)
                {
                    return schedule;
                }
            }

            return null;
        }


        public DateChosen AddDatesToList(string[] value , DateChosen dateChosen , CalenderViewModel calenderViewModel)
        {
            List<DateTime> list = new List<DateTime>(); 
            var result = HttpContext.Session.GetObject<ScheduleViewModel>("v");
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != null)
                {
                    list.Add(DateTime.Parse(value[i]));
                }
            }

            

            foreach (var olddates in dateChosen.CosenDays)
            {
                if (!olddates.Month.Equals(calenderViewModel.CurrentDate.Month))
                {
                    list.Add(olddates);
                }
            
                
            }

            dateChosen.CosenDays = list;
            
            return dateChosen;

        }
        
        
    }
}
