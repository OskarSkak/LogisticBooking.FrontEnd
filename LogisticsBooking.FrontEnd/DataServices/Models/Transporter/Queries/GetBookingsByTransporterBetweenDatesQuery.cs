using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Queries
{
    public class GetBookingsByTransporterBetweenDatesQuery
    {
        public Guid TransporterId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}