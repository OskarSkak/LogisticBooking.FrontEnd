using LogisticsBooking.FrontEnd.DataServices.Models.Booking;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Validation
{
    public class BookingSuppliersOverlapValidationCommand
    {
        public BookingViewModel BookingViewModel { get; set; }
        public OrderViewModel OrderViewModel { get; set; }
    }
}