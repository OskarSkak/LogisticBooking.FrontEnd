using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.Client.admin
{
    public class AllUsers : PageModel
    {
        private readonly IApplicationUserDataService _applicationUserDataService;
        public ListApplicationUserViewModels ApplicationUserViewModels { get; set; }

        public AllUsers(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
        } 
        
        public async Task OnGet()
        {
            ApplicationUserViewModels = await _applicationUserDataService.GetAllUsers();
            Console.WriteLine("");
        }
    }
}