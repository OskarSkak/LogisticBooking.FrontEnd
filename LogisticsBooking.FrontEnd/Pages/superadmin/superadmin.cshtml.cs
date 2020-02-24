using System;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LogisticsBooking.FrontEnd.Pages.superadmin
{
    public class superadmin : PageModel
    {
        private readonly ICustomerDataService _customerDataService;

        public CreateCustomerCommand CreateCustomerCommand { get; set; }

        [TempData]
        public string Message { get; set; }

        public bool IsMessageFilled => !String.IsNullOrEmpty(Message);
        
        public superadmin(ICustomerDataService customerDataService)
        {
            _customerDataService = customerDataService;
        }
        
        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostCreateCustomer(CreateCustomerCommand CreateCustomerCommand)
        {
            var result = await _customerDataService.CreateCustomer(CreateCustomerCommand);

            if (result.IsSuccesfull)
            {
                Message = "Kunden er oprettet";
                return Page();
            }
            Message = "Der skete en fejl";
            return Page();
            
        }
    }
}