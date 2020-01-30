using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private ITransporterDataService transporterDataService;
        
       
        public IndexModel(ITransporterDataService _transporterDataService)
        {
            transporterDataService = _transporterDataService;
        }
        public IActionResult OnGet()
        {
            
            return new RedirectToPageResult("User/Home");
        }
        
        public async Task  OnGetLogoutAsync()
        {
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc");

            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //
            return new RedirectResult("User/Home");
        }
    }
}
