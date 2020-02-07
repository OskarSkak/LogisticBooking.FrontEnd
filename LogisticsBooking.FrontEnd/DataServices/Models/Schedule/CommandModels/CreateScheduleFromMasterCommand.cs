using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class CreateScheduleFromMasterCommand
    {
        public List<IntervalViewModel> IntervalViewModels { get; set; }
        public DateTime? ScheduleDay { get; set; }
        public Guid CreatedBy { get; set; }
        public int MischellaneousPallets { get; set; }
        public Shift Shifts { get; set; }
        public string Name { get; set; }
        
        public bool IsStandard { get; set; }
        
        public Guid ScheduleId { get; set; }
        
        public List<DayViewModel> ActiveDays { get; set; } 
    }
}