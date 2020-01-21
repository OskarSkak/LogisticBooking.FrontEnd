using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels
{
    public class MasterSchedulesStandardViewModel
    {
        public MasterSchedulesStandardViewModel()
        {
            MasterScheduleStandardViewModels = new List<MasterScheduleStandardViewModel>();
        }
        public List<MasterScheduleStandardViewModel> MasterScheduleStandardViewModels { get; set; }
    }
}