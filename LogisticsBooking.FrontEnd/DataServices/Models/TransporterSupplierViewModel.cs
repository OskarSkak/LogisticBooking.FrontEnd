using System;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class TransporterSupplierViewModel
    {
        public TransporterViewModel Transporter { get; set; }
        public Guid TransporterViewModelId { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public Guid SupplierViewModelId { get; set; }  
    }
}