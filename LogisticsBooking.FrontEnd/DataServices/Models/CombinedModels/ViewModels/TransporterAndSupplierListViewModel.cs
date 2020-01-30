using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;

namespace LogisticsBooking.FrontEnd.DataServices.Models.CombinedModels.ViewModels
{
    public class TransporterAndSupplierListViewModel
    {
        public List<TransporterViewModel> Transporters { get; set; }
        public List<SupplierViewModel> Suppliers { get; set; }
    }
}