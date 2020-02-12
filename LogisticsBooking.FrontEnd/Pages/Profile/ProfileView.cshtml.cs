using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Presentation;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;
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
        [BindProperty] public ApplicationUserViewModel LoggedInUser { get; set; }
        [BindProperty] public bool OfficeRoleIsChecked { get; set; }
        [BindProperty] public bool WareHouseRoleIsChecked { get; set; }
        [BindProperty] public bool TransporterRoleIsChecked { get; set; }
        [BindProperty] public bool ClientRoleIsChecked { get; set; }
        [BindProperty] public bool AdminRoleIsChecked { get; set; } //TODO: Form check for admin - can be attacked
        
        
        public ProfileViewModel(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
            ApplicationUserViewModels = _applicationUserDataService.GetAllUsers().Result;
            LoggedInUser = new ApplicationUserViewModel();
        }
        
        public async Task OnGet()
        {
            Roles = CreateSelectList();
            //ApplicationUserViewModels = await _applicationUserDataService.GetAllUsers();
            var LoggedInIdString = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var LoggedInId = Guid.Parse(LoggedInIdString);
            LoggedInUser = await _applicationUserDataService.GetUserById(new GetUserByIdCommand{Id = LoggedInId});
            foreach (var role in LoggedInUser.ActiveRoles)
            {
                if (role.Name.ToLower() == "kontor") OfficeRoleIsChecked = true;
                if (role.Name.ToLower() == "lager") WareHouseRoleIsChecked = true;
                if (role.Name.ToLower() == "transporter") TransporterRoleIsChecked = true;
                if (role.Name.ToLower() == "client") ClientRoleIsChecked = true;
                if (role.Name.ToLower() == "admin") AdminRoleIsChecked = true;
            }
        }

        public async Task<IActionResult> OnPostCreateAsync(CreateUserCommand createUserCommand)
        {
            var result = await _applicationUserDataService.CreateUser(createUserCommand);
            if (result.IsSuccesfull) Message = "User created. Check email for confirmation link";
            return Page();
        }

        public async Task<IActionResult> OnPostUpdateAsync(ApplicationUserViewModel LoggedInUser)
        {
            var LoggedInIdString = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
            var Roles = new List<string>();
            var cmd = new UpdateUserWithRolesCommand();
            cmd.ApplicationUserId = LoggedInIdString;
            
            if (ClientRoleIsChecked)Roles.Add("client");
            if (OfficeRoleIsChecked) Roles.Add("kontor");
            if (TransporterRoleIsChecked) Roles.Add("transporter");
            if (WareHouseRoleIsChecked) Roles.Add("lager");
            if (AdminRoleIsChecked) Roles.Add("admin");
            
            if (!string.IsNullOrWhiteSpace(LoggedInUser.Name)) cmd.Name = LoggedInUser.Name;
            if (!string.IsNullOrWhiteSpace(LoggedInUser.Email)) cmd.Email = LoggedInUser.Email;
            if (Roles.Count != 0) cmd.Roles = Roles;

            var result = await _applicationUserDataService.UpdateUser(cmd); //TODO: Error handle
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