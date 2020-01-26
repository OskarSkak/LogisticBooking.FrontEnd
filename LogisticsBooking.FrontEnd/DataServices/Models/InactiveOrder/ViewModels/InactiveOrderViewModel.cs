using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;

namespace LogisticsBooking.FrontEnd.DataServices.Models.InactiveOrder.ViewModels
{
    public class InactiveOrderViewModel
    {
        public Guid InternalId { get; set; }
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime? BookingTime { get; set; }
        public Guid TransporterId { get; set; }
        public TransporterViewModel Transporter { get; set; }
        public List<InactiveOrderViewModel> InactiveOrders {get; set;}
        public int Port { get; set; }
    }
}