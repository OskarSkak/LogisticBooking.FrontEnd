using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.User
{
    [Authorize]
    public class Home : PageModel
    {
        [Authorize]
        public async Task<IActionResult> OnGet()
        {
            
            if (User.HasClaim("role", "client"))
            {
                return new  RedirectToPageResult("/da/Client/Dashboard");
            }  
            if (User.HasClaim("role", "kontor"))
            {
                return new  RedirectToPageResult("/da/Client/Dashboard");
            }  
            if (User.HasClaim("role", "lager"))
            {
                return new  RedirectToPageResult("/da/Client/Dashboard");
            }  
            
            if (User.HasClaim("role" , "transporter"))
            {
                return RedirectToPage("/da/Transporter/Booking/BookOrder");
            } 
            if (User.HasClaim("role" , "admin"))
            {
                return new  RedirectResult("/da/Client/Dashboard");
            } 
            return new RedirectToPageResult("Error");
        }

        public async Task OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");
        }
    }
}