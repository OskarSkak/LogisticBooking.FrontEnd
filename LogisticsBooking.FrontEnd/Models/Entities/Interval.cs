using System;
using System.Collections.Generic;


namespace LogisticBooking.API.Domain.Entities
{

    public class Interval
    {

        public Interval()
        {
            Bookings = new List<Booking>();
        }
        
        // Internal ID 
        public Guid IntervalId { get; set;  }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsBooked { get; set; }
        public int BottomPallets { get; set; }
        public int RemainingPallets { get; set; }
        
        
        
        // ************************************* Relations ******************************

        
        public Guid ScheduleId { get; set; }
        public Schedule Schedule { get; set; }
        
    
        public List<Booking> Bookings { get;} 
    }
}
