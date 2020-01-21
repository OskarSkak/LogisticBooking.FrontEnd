using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    [Authorize]
    public class ClientIndexModel : PageModel
    {
        public void OnGet()
        {
        }

        public Task OnGetClaimsAsync()
        {
            foreach (var VARIABLE in User.Claims)
            {
                Console.WriteLine("Claim type: " +  VARIABLE.Type + "   Claim value " + VARIABLE.Value );
            }

            return Task.FromResult(0);
        }
    }
}