using System;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;

namespace LogisticsBooking.FrontEnd.DataServices.Utilities
{
    public class BookingUtil
    {
        public static BookingViewModel RemoveDates(BookingViewModel booking)
        {
            booking.ActualArrival = default(DateTime).Add(booking.ActualArrival.TimeOfDay);
            booking.EndLoading = default(DateTime).Add(booking.EndLoading.TimeOfDay);
            booking.StartLoading = default(DateTime).Add(booking.StartLoading.TimeOfDay);

            return booking;
        }
    }
}