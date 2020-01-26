using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.Commands
{
    public class UpdateInactiveBookingCommand
    {
        public Guid InternalId { get; set; }
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime? BookingTime { get; set; }
        public Guid TransporterId { get; set; }
        public int Port { get; set; }
    }
}