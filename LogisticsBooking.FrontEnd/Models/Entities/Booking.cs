using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace LogisticBooking.API.Domain.Entities
{
 
    public class Booking
    {


        public Booking()
        {
            Orders = new List<Order>();
        }
        
        [Key]
        // Internal ID 
        public Guid InternalId { get; set; }
        
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime? BookingTime { get; set; }
        
        public int Port { get; set; }
        public DateTime? ActualArrival { get; set; }
        public DateTime? StartLoading { get; set; }
        public DateTime? EndLoading { get; set; }
        
        public string Email { get; set; }
        
        
        // ************************************* Relations ******************************
        
        
        
        // Order
        public  List<Order> Orders { get; }
        public Guid IntervalId { get; set; }
        public Interval Interval { get; set; }
        public Guid TransporterId { get; set; }
        public Transporter Transporter { get; set; }
        
        
    }
}
