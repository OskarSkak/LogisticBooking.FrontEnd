using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    public class Suppliers1Model : PageModel
    {
        private readonly ILogisticBookingApiDatabase _logisticBookingApiDatabase;
        private readonly IMapper _mapper;
        private readonly ISupplierDataService _supplierDataService;
        
        
        [BindProperty] 
        public SuppliersListViewModel SuppliersListView { get; set; }
        
        [TempData]
        public string ResponseMessage { get; set; }
        
        public bool ShowResponseMessage  => !String.IsNullOrEmpty(ResponseMessage);

        public Suppliers1Model(ILogisticBookingApiDatabase logisticBookingApiDatabase , IMapper mapper , ISupplierDataService supplierDataService)
        {
            _logisticBookingApiDatabase = logisticBookingApiDatabase;
            _mapper = mapper;
            _supplierDataService = supplierDataService;
        }
        
        public async Task<IActionResult> OnGet()
        {

            SuppliersListView = await PopulateList(_supplierDataService);
            

                return Page();
        }

        public async Task OnGetUser()
        {
            
        }

        private static async Task<SuppliersListViewModel> PopulateList(ISupplierDataService supplierDataService)
        {
            var suppliersEnumerable = await supplierDataService.ListSuppliers(0, 0);
            var suppliersList = suppliersEnumerable;
            return suppliersList;
        }
    }
}