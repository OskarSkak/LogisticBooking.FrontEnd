using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoreLinq;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Client.Schedule
{
    public class AllSchedules : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly ITransporterDataService _transporterDataService;

        [BindProperty]
        public MasterSchedulesStandardViewModel MasterSchedulesStandardViewModel { get; set; }
        
        [BindProperty]
        public List<RadioItems> RadioItems { get; set; }
        
        [BindProperty] 
        public MasterScheduleStandardViewModel masterScheduleStandardViewModel { get; set; }
        
        
        public List<SelectListItem> RadioList { get; set; }

        public AllSchedules(IMasterScheduleDataService masterScheduleDataService, ITransporterDataService transporterDataService)
        {
            _masterScheduleDataService = masterScheduleDataService;
            _transporterDataService = transporterDataService;
        }

        public async Task OnGet()
        {
            var result = await _masterScheduleDataService.GetAllSchedules();
            MasterSchedulesStandardViewModel = result;
            MasterSchedulesStandardViewModel.MasterScheduleStandardViewModels =
                result.MasterScheduleStandardViewModels.OrderBy(e => e.Shifts).ToList();
            
            RadioList = new List<SelectListItem>();

            foreach (var schedule in MasterSchedulesStandardViewModel.MasterScheduleStandardViewModels)
            {
                RadioList.Add(new SelectListItem
                {
                    Selected = schedule.IsActive,
                    Value = schedule.MasterScheduleStandardId.ToString()
                });
            }
            
  
            
        }

        public async void OnPost()
        {
            Console.WriteLine();
        }

        public async Task<IActionResult> OnPostChangeActiveAsync(string isActive ,  Guid id , Shift shift)
        {
            var result = await _masterScheduleDataService.SetMasterScheduleActive(new SetMasterScheduleStandardActiveCommand
                {MasterScheduleStandardToActive = id , Shift = shift});

            if (result.IsSuccesfull)
            {
                return new RedirectToPageResult("");
            }
            
            return new RedirectToPageResult("");
        }

        public async Task<IActionResult> OnPostMakeInactive(string isActive, Guid id, Shift shift)
        {
            var result = await _masterScheduleDataService.SetMasterScheduleInactive(new SetScheduleInactiveCommand()
                {MasterScheduleStandardToInactive = id , Shift = shift});

            if (result.IsSuccesfull)
            {
                return new RedirectToPageResult("");
            }
            
            return new RedirectToPageResult("");
        }

        public async Task<JsonResult> OnGetTest()
        {
            var Schedules = await _masterScheduleDataService.GetAllSchedules();

            var list = new List<IntervalViewModel>();

            foreach (var schedule in Schedules.MasterScheduleStandardViewModels)
            {
                schedule.MasterIntervalStandardViewModels =   schedule.MasterIntervalStandardViewModels.OrderBy(x => x.StartTime).ToList();
            }
            
            var json = new JsonResult(Schedules);
           
            return json;

        }

        public async Task<IActionResult> OnPostDeleteMasterScheduleAsync(Guid id)
        {
            var result = await _masterScheduleDataService.DeleteMasterScheduleStandard(id);
            return new RedirectToPageResult("");
        }
        
        
        

       
    }

    public class RadioItems
    {
        public Guid MasterScheduleId { get; set; }
        public bool IsActive { get; set; }
    }
}
