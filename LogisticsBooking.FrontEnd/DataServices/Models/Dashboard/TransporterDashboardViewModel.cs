using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Dashboard
{
    public class TransporterDashboardViewModel
    {
        public List<BookingViewModel> BookingsToday { get; set; }
    }
}