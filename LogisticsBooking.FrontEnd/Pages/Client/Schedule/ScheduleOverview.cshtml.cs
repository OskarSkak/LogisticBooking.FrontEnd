using System;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{

    public class ScheduleOverviewIndexModel : PageModel
    {
        private readonly IScheduleDataService _scheduleDataService;
        private readonly IMasterScheduleDataService _masterScheduleDataService;

        [TempData] public String Message { get; set; }

        [TempData] public string CompleteMessage { get; set; }


        [BindProperty]
        public MasterSchedulesStandardViewModel MasterSchedulesStandardViewModel { get; set; }
        

        [BindProperty]
        public CalenderViewModel CalenderViewModel { get; set; }
        
        public SchedulesListViewModel SchedulesListViewModel { get; set; }

        public ScheduleOverviewIndexModel(IScheduleDataService scheduleDataService , IMasterScheduleDataService masterScheduleDataService)
        {
            _scheduleDataService = scheduleDataService;
            _masterScheduleDataService = masterScheduleDataService;
        }
        public async Task<IActionResult> OnGetAsync()
        {

            MasterSchedulesStandardViewModel = await _masterScheduleDataService.GetActiveMasterSchedule();

            
            
            SchedulesListViewModel = await _scheduleDataService.GetSchedules();

            if (SchedulesListViewModel == null)
            {
                SchedulesListViewModel = new SchedulesListViewModel();
            }
            
            

            return Page();
        }

        
    }
}
