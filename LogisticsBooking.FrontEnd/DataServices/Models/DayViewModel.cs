using System;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class DayViewModel
    {
        public Guid DayId { get; set; }
        public DayOfWeek ActiveDay { get; set; }
        public bool IsActive { get; set; }
        public ScheduleViewModel ScheduleViewModel { get; set; }
        public Guid ScheduleId { get; set; }
    }
}