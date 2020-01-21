using System;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands
{
    public class SetMasterScheduleStandardActiveCommand
    {
        public Guid MasterScheduleStandardToActive { get; set; }
        public Shift Shift { get; set; }
    }
}