using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class DeleteBookingCommand
    {
        public Guid BookingId { get; set; }
    }
}