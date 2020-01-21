using System;


namespace LogisticBooking.API.Domain.Entities
{
    public class Order
    {
         // Internal ID 
        public Guid OrderId { get; set; }
        
        
        public string Comment { get; set; }     
        public int TotalPallets { get; set; }
        public int BottomPallets { get; set; }
        public string ExternalId { get; set; }  
        
        public string CustomerNumber { get; set; }
        public string OrderNumber { get; set; }
        public int WareNumber { get; set; }
        public string InOut { get; set; }
        
        
        // ************************************* Relations ******************************
        
        
        
        public Guid SupplierId { get; set; }
        
        public Supplier Supplier { get; set; }
        public Guid BookingId { get; set; }
        
       
        public Booking Booking { get; set; }
        
    }
}