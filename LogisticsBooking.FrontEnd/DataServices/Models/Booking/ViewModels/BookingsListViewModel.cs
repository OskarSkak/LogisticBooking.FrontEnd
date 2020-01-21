using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Booking
{
    public class BookingsListViewModel
    {
        public BookingsListViewModel()
        {
            Bookings = new List<BookingViewModel>();
        }
        
        public List<BookingViewModel> Bookings { get; set; }
    }
}