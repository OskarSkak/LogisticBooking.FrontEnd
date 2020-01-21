using System;
using System.Collections.Generic;

namespace LogisticBooking.API.Domain.Entities
{
    
    public enum Shift
    {
        Day,Night
    }

    public class Schedule
    {

        public Schedule()
        {
            Intervals = new List<Interval>();
        }
        
        // Internal ID 
        public Guid ScheduleId { get; set; }
        
        public DateTime? ScheduleDay { get; set; }
        public Guid CreatedBy { get; set; }
        public int MischellaneousPallets { get; set; }
        public Shift Shifts { get; set; }
        public string Name { get; set; }
        
        public bool IsStandard { get; set; }
    
        
        
        // ************************************* Relations ******************************
        public  List<Interval> Intervals { get; set; }
        
    }
}