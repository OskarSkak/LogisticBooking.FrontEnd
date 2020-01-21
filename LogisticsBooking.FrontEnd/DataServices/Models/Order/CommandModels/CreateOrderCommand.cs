using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class CreateOrderCommand
    {
        public Guid SupplierId { get; set; }
        
        public int TotalPallets { get; set; }
        
        public int BottomPallets { get; set; }
        
        public string OrderNumber { get; set; }
        
        public string Comments { get; set; }
        
        public string ExternalId { get; set; }
        
        public string InOut { get; set; }
        
    }
}