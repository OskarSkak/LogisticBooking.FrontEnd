using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    public class TransportersModel : PageModel
    {
        private ITransporterDataService transporterDataService;
        [BindProperty] public List<TransporterViewModel> Transporters { get; set; }
        
        [TempData]
        public String ResponseMessage { get; set; }
        
        public bool ShowResponseMessage => !String.IsNullOrEmpty(ResponseMessage);

        public TransportersModel(ITransporterDataService _transporterDataService)
        {
            transporterDataService = _transporterDataService;
            
        }
        public async Task<IActionResult> OnGet()
        {
            Transporters = new List<TransporterViewModel>();
            Transporters = await PopulateList(transporterDataService, Transporters);
            return new RedirectToPageResult("Error");
        }

        private  async Task<List<TransporterViewModel>> PopulateList(ITransporterDataService transporterDataService, List<TransporterViewModel> Transporters)
        {
            var TransportersEnumerable = await transporterDataService.ListTransporters(0, 0);
            
            return TransportersEnumerable.Transporters;
        }
    }
}