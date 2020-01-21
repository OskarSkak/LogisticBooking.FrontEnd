using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogisticBooking.API.Domain.Entities
{
    public class DeletedBooking
    {
        [Key()] public Guid InternalId { get; set; }
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime? BookingTime { get; set; }
        public int Port { get; set; }
        public DateTime? ActualArrival { get; set; }
        public DateTime? StartLoading { get; set; }
        public DateTime? EndLoading { get; set; }
        public string Email { get; set; } 
        
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int BottomPallets { get; set; }
        
        public DeletedBooking()
        {
            Orders = new List<Order>();
        }
        
        public  List<Order> Orders { get; set; }
        public Guid TransporterId { get; set; }
        public Transporter Transporter { get; set; }
    }
}