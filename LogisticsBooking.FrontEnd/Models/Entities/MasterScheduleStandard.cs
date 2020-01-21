using System;
using System.Collections.Generic;

namespace LogisticBooking.API.Domain.Entities
{
    
    public class MasterScheduleStandard
    {
        public Guid MasterScheduleStandardId { get; set; }
        
        public Guid CreatedBy { get; set; }
        public int MischellaneousPallets { get; set; }
        public Shift Shifts { get; set; }
        public string Name { get; set; }
        
        public bool IsActive { get; set; }

        public  List<MasterIntervalStandard> MasterIntervalStandards { get; set; }
    }
}