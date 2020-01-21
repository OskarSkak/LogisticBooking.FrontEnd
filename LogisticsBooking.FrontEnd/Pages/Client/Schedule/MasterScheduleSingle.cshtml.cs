using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MoreLinq.Extensions;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class MasterScheduleSingle : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;

        [BindProperty]
        public MasterScheduleStandardViewModel MasterScheduleStandardViewModel { get; set; }

        public MasterScheduleSingle(IMasterScheduleDataService masterScheduleDataService)
        {
            _masterScheduleDataService = masterScheduleDataService;
        }
        public async Task OnGet(Guid id)
        {
          MasterScheduleStandardViewModel  =  await _masterScheduleDataService.GetMasterScheduleById(id);
        
          var result =  MasterScheduleStandardViewModel.MasterIntervalStandardViewModels.OrderBy(e => e.StartTime).ToList();
          MasterScheduleStandardViewModel.MasterIntervalStandardViewModels = result;
        }

        
        
    }
}