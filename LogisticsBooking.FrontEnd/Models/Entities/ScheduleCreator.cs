using System;

namespace LogisticBooking.API.Domain.Entities
{
    public class ScheduleCreator
    {
        public Guid ScheduleCreatorId { get; set; }
        
        public Schedule Schedule { get; set; }
        
    }
}