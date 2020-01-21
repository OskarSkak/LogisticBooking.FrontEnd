using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models.DeletedBooking.ViewModels
{
    public class DeletedBookingsListViewModel
    {
        public DeletedBookingsListViewModel()
        {
            DeletedBookings = new List<DeletedBookingViewModel>();
        }
        
        public List<DeletedBookingViewModel> DeletedBookings { get; set; }
    }
}