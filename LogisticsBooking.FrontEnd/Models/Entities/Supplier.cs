using System;
using System.Collections.Generic;

namespace LogisticBooking.API.Domain.Entities
{

    public class Supplier
    {
        public Supplier()
        {
            Orders = new List<Order>();
        } 
        
        
        // Internal ID
        public Guid SupplierId { get; set; }
        
        
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Name { get; set; }
        
        public DateTime ArriveTime { get; set; }

        public DateTime DeliveryStart { get; set; }
        public DateTime DeliveryEnd { get; set; }
        
        
        // ************************************* Relations ******************************
        
        public List<Order> Orders { get; }
       
    }
}