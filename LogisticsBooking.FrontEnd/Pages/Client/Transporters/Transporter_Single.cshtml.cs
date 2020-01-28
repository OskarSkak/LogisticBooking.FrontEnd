using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.commands;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace LogisticsBooking.FrontEnd.Pages.Client.Transporters
{
    public class Transporter_SingleModel : PageModel
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IMapper _mapper;
        private readonly ISupplierDataService _supplierDataService;


        [BindProperty] 
        public TransporterViewModel TransporterViewModel { get; set; }


        [BindProperty]
        public SuppliersListViewModel SupplierViewModel { get; set; }
        
        public List<SelectListItem> ActiveTransportersSelectList { get; set; }
        public SelectList SelectList { get; set; }
        
        public List<Guid> SelectedSupplier { get; set; }
        
        public List<Guid> ActiveSelectedSupplier { get; set; }
        
        [TempData] public String ResponseMessage { get; set; }

        public Transporter_SingleModel(ITransporterDataService transporterDataService, IMapper mapper , ISupplierDataService supplierDataService)
        {
            _transporterDataService = transporterDataService;
            _mapper = mapper;
            _supplierDataService = supplierDataService;
        }

        public async Task OnGetAsync(Guid id)
        {
            TransporterViewModel = await _transporterDataService.GetTransporterById(id);
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