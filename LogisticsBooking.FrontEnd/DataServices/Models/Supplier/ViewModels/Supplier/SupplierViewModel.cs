using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier
{
    public class SupplierViewModel
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        
        public string Name { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime ArriveTime { get; set; }
        // Leverings vindue for kunden
        public DateTime DeliveryStart { get; set; }
        public DateTime DeliveryEnd { get; set; }
        
        public List<TransporterSupplierViewModel> Transporters { get; set; }
        
    }
}