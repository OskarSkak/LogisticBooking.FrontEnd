using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class UpdateOrderCommand
    {
        public Guid OrderId { get; set; }
        public string Comment { get; set; }     
        public int TotalPallets { get; set; }
        public int BottomPallets { get; set; }
        public string ExternalId { get; set; }  
        public virtual Guid BookingId { get; set; }
        public string CustomerNumber { get; set; }
        public string OrderNumber { get; set; }
        public int WareNumber { get; set; }
        public string InOut { get; set; }
        public string SupplierName { get; set; }
    }
}