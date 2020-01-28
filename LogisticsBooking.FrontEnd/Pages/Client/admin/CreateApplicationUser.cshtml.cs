using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsBooking.FrontEnd.Pages.Client.admin
{
    public class CreateApplicationUser : PageModel
    {
        private readonly IApplicationUserDataService _applicationUserDataService;

        [TempData] public string Message { get; set; }



        public bool MessageIsNull => !String.IsNullOrEmpty(Message);

        public CreateUserCommand CreateUserCommand { get; set; }



        [BindProperty] public List<SelectListItem> Roles { get; set; }

        public CreateApplicationUser(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
        }

        public void OnGet()
        {
            Roles = CreateSelectList();

        }


        public async Task<IActionResult> OnPostCreateAsync(CreateUserCommand createUserCommand)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            
            if (createUserCommand.Role == "transporter")
            {
               
            } 
            var result = await _applicationUserDataService.CreateUser(createUserCommand);

            if (result.IsSuccesfull)
            {
                Message = "User created. Check email for confirmation link";
            }

            return Page();
        }

        public List<SelectListItem> CreateSelectList()
        {
            List<SelectListItem> roles = new List<SelectListItem>();

            roles.AddRange(new List<SelectListItem>
            {
                new SelectListItem("Kontor", "kontor"),
                new SelectListItem("Lager", "Lager"),
                new SelectListItem("Transportør", "transporter")
            });

            return roles;
        }
    }
    
}