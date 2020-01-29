using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;

namespace LogisticsBooking.FrontEnd.DataServices.Models.InactiveOrder.ViewModels
{
    public class InactiveOrderViewModel
    {
        public Guid OrderId { get; set; }
        public string Comment { get; set; }     
        public int TotalPallets { get; set; }
        public int BottomPallets { get; set; }
        public string ExternalId { get; set; }  
        public string CustomerNumber { get; set; }
        public string OrderNumber { get; set; }
        public int WareNumber { get; set; }
        public string InOut { get; set; }
        public Guid SupplierId { get; set; }
        public SupplierViewModel Supplier { get; set; }
        public Guid BookingId { get; set; }
    }
}