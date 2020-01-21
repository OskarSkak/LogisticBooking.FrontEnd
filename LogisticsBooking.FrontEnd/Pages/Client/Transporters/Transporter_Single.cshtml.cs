using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_SingleModel : PageModel
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IMapper _mapper;


        [BindProperty] 
        public TransporterViewModel TransporterViewModel { get; set; }


        [TempData] public String ResponseMessage { get; set; }

        public Transporter_SingleModel(ITransporterDataService transporterDataService, IMapper mapper)
        {
            _transporterDataService = transporterDataService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            TransporterViewModel = await _transporterDataService.GetTransporterById(id);
            

            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(TransporterViewModel transporterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _transporterDataService.UpdateTransporter(transporterViewModel.TransporterId, transporterViewModel );

            if (!result.IsSuccesfull)
            {
                return new RedirectToPageResult("Error");
            }

            ResponseMessage = "Opdatering af transportør var vellykket";
            return new RedirectToPageResult("./Transporters");
        }

        public async Task<IActionResult> OnPostDelete(TransporterViewModel transporterViewModel)
        {
            var result = await _transporterDataService.DeleteTransporter(TransporterViewModel.TransporterId);
            ResponseMessage = "Transportøren er slettet korrekt";
            return new RedirectToPageResult("./Transporters");
        }
    }
}