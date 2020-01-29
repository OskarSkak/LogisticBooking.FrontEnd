using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.commands;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class TransporterDetail : PageModel
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IMapper _mapper;
        private readonly ISupplierDataService _supplierDataService;
        private readonly ILogger<TransporterDetail> _logger;


        [BindProperty] 
        public TransporterViewModel TransporterViewModel { get; set; }


        [BindProperty]
        public SuppliersListViewModel SupplierViewModel { get; set; }
        
        public List<SelectListItem> ActiveTransportersSelectList { get; set; }
        public SelectList SelectList { get; set; }
        
        public List<Guid> SelectedSupplier { get; set; }
        
        public List<Guid> ActiveSelectedSupplier { get; set; }
        
        [TempData] public String ResponseMessage { get; set; }

        public TransporterDetail(ITransporterDataService transporterDataService, IMapper mapper , ISupplierDataService supplierDataService , ILogger<TransporterDetail> logger )
        {
            _transporterDataService = transporterDataService;
            _mapper = mapper;
            _supplierDataService = supplierDataService;
            _logger = logger;
        }

        public async Task OnGetAsync(string ok)
        {
            _logger.LogWarning("Ramte her!!!!!!!!!!");
            TransporterViewModel = await _transporterDataService.GetTransporterById(Guid.Parse(ok));
            SupplierViewModel = await _supplierDataService.ListSuppliers(0, 0);
            
            SelectList = new SelectList(SupplierViewModel.Suppliers, nameof(DataServices.Models.Supplier.Supplier.SupplierViewModel.SupplierId) , nameof(DataServices.Models.Supplier.Supplier.SupplierViewModel.Name));
            ActiveTransportersSelectList = new List<SelectListItem>();

            foreach (var supplier in TransporterViewModel.Suppliers)
            {
                ActiveTransportersSelectList.Add(new SelectListItem
                {
                    Text = supplier.Supplier.Name,
                    Value = supplier.SupplierViewModelId.ToString()
                });
            }
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

        public async Task<IActionResult> OnPostAddSupplier(Guid SelectedSupplier , TransporterViewModel transporterViewModel)
        {

            if (SelectedSupplier.Equals(Guid.Empty))
            {
                return new RedirectToPageResult("", new {id = transporterViewModel.TransporterId}); 
            }
           await _transporterDataService.AddSupplierToTransporter(new AddSupplierToTransporterCommand
            {
                SupplierId = SelectedSupplier,
                TransporterId = transporterViewModel.TransporterId
            });
            return new RedirectToPageResult("", new {id = transporterViewModel.TransporterId});
        }

        public async Task<IActionResult> OnPostRemoveSupplier(Guid ActiveSelectedSupplier,
            TransporterViewModel transporterViewModel)
        {
            if (ActiveSelectedSupplier.Equals(Guid.Empty))
            {
                return new RedirectToPageResult("", new {id = transporterViewModel.TransporterId}); 
            }

           var result = await _transporterDataService.RemoveSupplierFromTransporter(new RemoveSupplierFromTransporterCommand
            {
                SupplierId = ActiveSelectedSupplier,
                TransporterId = transporterViewModel.TransporterId
            });

            return new RedirectToPageResult("", new {id = transporterViewModel.TransporterId}); 
        }

        public async Task<IActionResult> OnPostDelete(TransporterViewModel transporterViewModel)
        {
            var result = await _transporterDataService.DeleteTransporter(TransporterViewModel.TransporterId);
            ResponseMessage = "Transportøren er slettet korrekt";
            return new RedirectToPageResult("./Transporters");
        }
    }
    
}