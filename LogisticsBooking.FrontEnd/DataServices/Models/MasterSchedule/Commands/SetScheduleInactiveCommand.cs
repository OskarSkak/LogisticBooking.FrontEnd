using System;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands
{
    public class SetScheduleInactiveCommand
    {
        public Guid MasterScheduleStandardToInactive { get; set; }
        public Shift Shift { get; set; }
    }
}