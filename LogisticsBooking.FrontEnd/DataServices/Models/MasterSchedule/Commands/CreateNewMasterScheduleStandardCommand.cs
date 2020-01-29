using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands
{
    public class CreateNewMasterScheduleStandardCommand 
    {
        public Guid MasterScheduleStandardId { get; set; }
        
        public Guid CreatedBy { get; set; }
        public int MischellaneousPallets { get; set; }
        public Shift Shifts { get; set; }
        public string Name { get; set; }
        
        public bool IsActive { get; set; }

        public  List<MasterIntervalStandardViewModel> MasterIntervalStandardViewModels { get; set; }
        
        public List<MasterDayViewModel> ActiveDays { get; set; }
    }
}