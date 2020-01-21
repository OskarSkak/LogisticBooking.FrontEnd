using System;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class CalenderViewModel
    { 
        
        public DateTime CurrentDate { get; set; }
        
        public DateTime FirstDateOfWeek { get; set; }
        
        public DateTime StartDate { get; set; }
        


        public CalenderViewModel()
        {
            CurrentDate = DateTime.Now;
            FirstDateOfWeek = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            StartDate = FirstDateOfWeek.AddDays(-(int)FirstDateOfWeek.DayOfWeek);
            
        }

        public void AdvanceMonth()
        {
            CurrentDate = CurrentDate.AddMonths(1);
            FirstDateOfWeek = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            StartDate = FirstDateOfWeek.AddDays(-(int)FirstDateOfWeek.DayOfWeek);
        }

        public void DecreaseMonth()
        {
            CurrentDate = CurrentDate.AddMonths(-1);
            FirstDateOfWeek = new DateTime(CurrentDate.Year, CurrentDate.Month, 1);
            StartDate = FirstDateOfWeek.AddDays(-(int)FirstDateOfWeek.DayOfWeek);
        }
    }
    
    
   
}