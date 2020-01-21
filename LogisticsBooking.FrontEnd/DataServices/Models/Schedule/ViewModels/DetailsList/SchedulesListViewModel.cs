using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList
{
    public class SchedulesListViewModel
    {
        public SchedulesListViewModel()
        {
            Schedules = new List<ScheduleViewModel>();
        }
        public List<ScheduleViewModel> Schedules { get; set; } 
    }
}