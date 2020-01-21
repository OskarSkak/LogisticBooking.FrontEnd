using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class CreateBookingCommand
    {
        public int ExternalId { get; set; }

        public DateTime? DeliveryDate { get; set; }
        
        public int TotalPallets { get; set; }
        
        public Guid TransporterId { get; set; }
        
        public List<CreateOrderCommand> CreateOrderCommand { get; set; }
        
        public Guid IntervalId { get; set; }
        
        public string InOut { get; set; }
    }
}