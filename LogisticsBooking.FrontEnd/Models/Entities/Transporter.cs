using System;
using System.Collections.Generic;

namespace LogisticBooking.API.Domain.Entities
{
    public class Transporter
    {
        public Transporter()
        {
            Bookings = new List<Booking>();
        }
        // Internal ID 
        public Guid TransporterId { get; set; }
        
        
        public string Email { get; set; }
        public int Telephone { get; set; }
        public string Address { get; set; }
        public string Name { get; set; }
        
        
        // ************************************* Relations ******************************
        
        public   List<Booking> Bookings { get; }
        public List<DeletedBooking> DeletedBookings { get; }
    }
}