using System.Collections.Generic;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class OrdersListViewModel
    {

        public OrdersListViewModel()
        {
            Orders = new List<OrderViewModel>();
        }
        
        public List<OrderViewModel> Orders { get; set; }
    }
}