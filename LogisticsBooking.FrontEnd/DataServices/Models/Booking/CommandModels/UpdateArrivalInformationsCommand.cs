using System;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Booking.CommandModels
{
    public class UpdateArrivalInformationsCommand
    {
        public DateTime ActualArrival { get; set; }
        public DateTime StartLoading { get; set; }
        public DateTime EndLoading { get; set; }
        public Guid InternalId { get; set; }

    }
}