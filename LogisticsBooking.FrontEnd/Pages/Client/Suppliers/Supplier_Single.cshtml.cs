using System;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Suppliers
{
    public class Supplier_SingleModel : PageModel
    {
        private readonly ISupplierDataService _supplierDataService;
        private readonly IMapper _mapper;

        [BindProperty] 
        public SupplierViewModel SupplierViewModel { get; set;}
        
        
        public Supplier_SingleModel(ISupplierDataService supplierDataService , IMapper mapper )
        {
            _supplierDataService = supplierDataService;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            SupplierViewModel = await _supplierDataService.GetSupplierById(id);
            
            
            return Page();
        }

        public async Task<IActionResult> OnPostUpdate(SupplierViewModel supplierViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var result = await _supplierDataService.UpdateSupplier(SupplierViewModel.SupplierId, supplierViewModel);

            if (result.IsSuccesfull) return new RedirectToPageResult("./Suppliers");
            
            return new RedirectToPageResult("Error");
        }
        
        public async Task<IActionResult> OnPostDelete(Guid id)
        {
            var supplier = await _supplierDataService.GetSupplierById(id);

            var result = await _supplierDataService.DeleteSupplier(supplier.SupplierId);
            
            return new RedirectToPageResult("./Suppliers");
        }
    }
}