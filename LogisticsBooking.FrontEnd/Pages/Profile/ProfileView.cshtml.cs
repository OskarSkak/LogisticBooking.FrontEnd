using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsBooking.FrontEnd.Pages.Profile
{
    public class ProfileViewModel : PageModel
    {
        private readonly IApplicationUserDataService _applicationUserDataService;
        public CreateUserCommand CreateUserCommand { get; set; }
        [BindProperty] public List<SelectListItem> Roles { get; set; }
        [TempData] public string Message { get; set; }
        public bool MessageIsNull => !String.IsNullOrEmpty(Message);
        [BindProperty] public ListApplicationUserViewModels ApplicationUserViewModels { get; set; }
        
        public ProfileViewModel(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
            ApplicationUserViewModels = _applicationUserDataService.GetAllUsers().Result;
        }
        
        
        public async void OnGet()
        {
            Roles = CreateSelectList();
            //ApplicationUserViewModels = await _applicationUserDataService.GetAllUsers();
        }

        public async Task<IActionResult> OnPostCreateAsync(CreateUserCommand createUserCommand)
        {
            var result = await _applicationUserDataService.CreateUser(createUserCommand);
            if (result.IsSuccesfull) Message = "User created. Check email for confirmation link";
            
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