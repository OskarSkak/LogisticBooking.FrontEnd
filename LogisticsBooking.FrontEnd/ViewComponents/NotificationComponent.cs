using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsBooking.FrontEnd.ViewComponents
{


    public class NotificationComponentModel
    {
        public InactiveBookingListViewModel InactiveBookingListViewModel { get; set; }
    }
    
    public class NotificationComponent : ViewComponent
    {
        private readonly IInactiveBookingDataService _inactiveBookingsDataService;

        public NotificationComponent(IInactiveBookingDataService inactiveBookingsDataService)
        {
            _inactiveBookingsDataService = inactiveBookingsDataService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var result = await _inactiveBookingsDataService.GetInactiveBookings();
            
            NotificationComponentModel notificationComponent = new NotificationComponentModel();
            notificationComponent.InactiveBookingListViewModel = result;
            
            return View(notificationComponent);
        }
    }
}