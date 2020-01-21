using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList
{
    public class SuppliersListViewModel
    {

        public SuppliersListViewModel()
        {
            Suppliers = new List<SupplierViewModel>();
        }
        public List<SupplierViewModel> Suppliers { get; set; }
    }
}