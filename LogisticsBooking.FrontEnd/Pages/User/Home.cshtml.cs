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
                return new  RedirectToPageResult("/Client/Dashboard");
            }  
            if (User.HasClaim("role", "kontor"))
            {
                return new  RedirectToPageResult("/Client/Dashboard");
            }  
            if (User.HasClaim("role", "lager"))
            {
                return new  RedirectToPageResult("/Client/Dashboard");
            }  
            
            if (User.HasClaim("role" , "transporter"))
            {
                return RedirectToPage("/Transporter/Booking/BookOrder");
            } 
            if (User.HasClaim("role" , "admin"))
            {
                return new  RedirectResult("/Client/Dashboard");
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