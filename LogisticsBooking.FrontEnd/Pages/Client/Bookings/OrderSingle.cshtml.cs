using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
    public class OrderSingleModel : PageModel
    {
        private IOrderDataService _orderDataService;
        [BindProperty] public OrderViewModel OrderViewModel { get; set; }
        
        [BindProperty]
        public Guid BookingId { get; set; }
        
        [TempData]
        public String Message { get; set; }

        
        public OrderSingleModel(IOrderDataService orderDataService)
        {
            _orderDataService = orderDataService;
        }

        public async Task OnGetAsync(string id , string bookingId)
        {
            OrderViewModel = await _orderDataService.GetOrderById(Guid.Parse(id));
            BookingId = Guid.Parse(bookingId);
        }

        public async Task<IActionResult> OnPostUpdate(string ViewComment, string ViewCustomerNumber, string ViewOrderNumber, 
            int ViewWareNumber, int ViewBottomPallets, string ViewExternalId, string ViewInOut, string id, int ViewTotalPallets , string bookingId)
        {
            var order = new UpdateOrderCommand
            {
                Comment = ViewComment, 
                CustomerNumber = ViewCustomerNumber, 
                OrderNumber = ViewOrderNumber, 
                WareNumber = ViewWareNumber, 
                BottomPallets = ViewBottomPallets, 
                ExternalId = ViewExternalId, 
                InOut = ViewInOut,
                OrderId = Guid.Parse(id),
                TotalPallets = ViewTotalPallets
            };

            var result = await _orderDataService.UpdateOrder(order);

            if (!result.IsSuccesfull) return new RedirectToPageResult("Error");
            
            Message = "Ordren er opdateret korrekt";
            return new RedirectToPageResult("BookingSingle" , new {id = bookingId , culture = CultureInfo.CurrentCulture.Name});
        }

        public async Task<IActionResult> OnPostDelete(string id , string bookingId)
        {
            var result = await _orderDataService.DeleteOrder(Guid.Parse(id));
            Message = "Ordren er slettet";
            return new RedirectToPageResult("BookingSingle" , new {id = bookingId , culture = CultureInfo.CurrentCulture.Name});
        }
        

       
        
    }
}
