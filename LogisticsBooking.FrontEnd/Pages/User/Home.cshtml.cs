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
                return RedirectToPage("/Client/Dashboard");
            }  
            if (User.HasClaim("role", "kontor"))
            {
                return RedirectToPage("/Client/Dashboard");
            }  
            if (User.HasClaim("role", "lager"))
            {
                return RedirectToPage("/Client/Dashboard");
            }  
            
            if (User.HasClaim("role" , "transporter"))
            {
                return RedirectToPage("/Transporter/Booking/BookOrder");
            } 
            if (User.HasClaim("role" , "admin"))
            {
                return RedirectToPage("/Client/Dashboard");
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