using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class OrderDeleteModel : PageModel
    {
        private IOrderDataService _orderDataService;
        [BindProperty] public OrderViewModel OrderViewModel { get; set; }
        
        public OrderDeleteModel(IOrderDataService orderDataService)
        {
            _orderDataService = orderDataService;
        }

        public async Task OnGetAsync(string id)
        {
            OrderViewModel = await _orderDataService.GetOrderById(Guid.Parse(id));
        }

        public async Task<IActionResult> OnPostDelete(string id)
        {
            var result = await _orderDataService.DeleteOrder(Guid.Parse(id));

            if (!result.IsSuccesfull) return new RedirectToPageResult("Error");
            return new RedirectToPageResult("BookingOverviewAdmin");
        }
        

       
        
    }
}
