using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier
{
    public class CreateSupplierViewModel
    {
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Name { get; set; }
        
        public DateTime ArriveTime { get; set; }
        // Leverings vindue for kunden
        public DateTime DeliveryStart { get; set; }
        public DateTime DeliveryEnd { get; set; }
    
    }
}