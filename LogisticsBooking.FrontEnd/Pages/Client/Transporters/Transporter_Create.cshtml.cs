using System;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_CreateModel : PageModel
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IMapper _mapper;
        private readonly IApplicationUserDataService _applicationUserDataService;


        [BindProperty]
        public TransporterViewModel TransporterViewModel { get; set; }
        
        [TempData]
        public String ResponseMessage { get; set; }
        
        public Transporter_CreateModel(ITransporterDataService transporterDataService , IMapper mapper , IApplicationUserDataService applicationUserDataService)
        {
            _transporterDataService = transporterDataService;
            _mapper = mapper;
            _applicationUserDataService = applicationUserDataService;
        }

        public void OnGet()
        {
            
        }

        public async Task<ActionResult> OnPost(TransporterViewModel transporterViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            var transporterId = Guid.NewGuid();
            transporterViewModel.TransporterId = transporterId;
            
            var result = await _transporterDataService.CreateTransporter(transporterViewModel);

            if (result.IsSuccesfull)
            {
                var transporter = new CreateUserCommand();
                transporter.Email = transporterViewModel.Email;
                transporter.Name = transporterViewModel.Email;
                transporter.Id = transporterId;
                transporter.Role = "transporter";
                var createTransporterResult = await _applicationUserDataService.CreateTransporter(transporter);
                if (createTransporterResult.IsSuccesfull)
                {
                    
                    return Page();
                }

                await _transporterDataService.DeleteTransporter(transporterId);
                return BadRequest();
            }
            
            
            ResponseMessage = "Transportøren er oprettet";
            return new RedirectResult("Transporters");
        }
    }
}