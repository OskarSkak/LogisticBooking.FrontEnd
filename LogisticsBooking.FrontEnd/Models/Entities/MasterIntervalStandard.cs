using System;

namespace LogisticBooking.API.Domain.Entities
{
    public class MasterIntervalStandard
    {
        public Guid MasterIntervalStandardId { get; set;  }
        
        public DateTime? StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }

        public int BottomPallets { get; set; }
        
           
        // ************************************* Relations ******************************

        
        public Guid MasterScheduleStandardId { get; set; }
        
        public MasterScheduleStandard MasterScheduleStandard { get; set; }
    }
}