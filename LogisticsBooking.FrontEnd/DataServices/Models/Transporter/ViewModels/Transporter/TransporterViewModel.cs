using System;
using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter
{
    public class TransporterViewModel
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        
        public Guid TransporterId { get; set; }
        
        public List<TransporterSupplierViewModel> Suppliers { get; set; }
    }
}