using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels
{
    public class InactiveBookingListViewModel
    {
        public List<InactiveBookingViewModel> InactiveBookings { get; set; }

        public InactiveBookingListViewModel()
        {
            InactiveBookings = new List<InactiveBookingViewModel>();
        }
    }
}