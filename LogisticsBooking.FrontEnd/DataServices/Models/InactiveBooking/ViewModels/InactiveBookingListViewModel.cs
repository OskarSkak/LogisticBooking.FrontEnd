using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels
{
    public class InactiveBookingListViewModel
    {
        public List<InactiveBookingViewModel> InactiveBookingViewModels { get; set; }

        public InactiveBookingListViewModel()
        {
            InactiveBookingViewModels = new List<InactiveBookingViewModel>();
        }
    }
}