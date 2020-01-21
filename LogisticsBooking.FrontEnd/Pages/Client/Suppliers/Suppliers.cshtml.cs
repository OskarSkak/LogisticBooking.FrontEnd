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
    public class SuppliersModel : PageModel
    {
        private readonly ILogisticBookingApiDatabase _logisticBookingApiDatabase;
        private readonly IMapper _mapper;
        private readonly ISupplierDataService _supplierDataService;
        
        
        [BindProperty] 
        public SuppliersListViewModel SuppliersListView { get; set; }
        
        [TempData]
        public string ResponseMessage { get; set; }
        
        public bool ShowResponseMessage  => !String.IsNullOrEmpty(ResponseMessage);

        public SuppliersModel(ILogisticBookingApiDatabase logisticBookingApiDatabase , IMapper mapper)
        {
            _logisticBookingApiDatabase = logisticBookingApiDatabase;
            _mapper = mapper;
        }
        
        public async Task<IActionResult> OnGet()
        {
            var suppliers = await _logisticBookingApiDatabase.Suppliers
                .ProjectTo<SupplierViewModel>(_mapper.ConfigurationProvider).ToListAsync();
            
            SuppliersListView =  new SuppliersListViewModel
            {
                Suppliers = suppliers
            };
           
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