using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsBooking.FrontEnd.Pages.Client.admin
{
    public class AdminOverviewModel : PageModel
    {
        private readonly IApplicationUserDataService _applicationUserDataService;
        public CreateUserCommand CreateUserCommand { get; set; }
        [BindProperty] public List<SelectListItem> Roles { get; set; }
        [TempData] public string Message { get; set; }
        public bool MessageIsNull => !String.IsNullOrEmpty(Message);
        [BindProperty] public ListApplicationUserViewModels ApplicationUserViewModels { get; set; }
        [BindProperty] public bool OfficeRoleIsChecked { get; set; }
        [BindProperty] public bool WareHouseRoleIsChecked { get; set; }
        [BindProperty] public bool TransporterRoleIsChecked { get; set; }
        [BindProperty] public bool ClientRoleIsChecked { get; set; }
        [BindProperty] public bool AdminRoleIsChecked { get; set; } //TODO: Form check for admin - can be attacked
        [BindProperty] public List<ApplicationUserWithRoleBoolsViewModel> BoolUsers { get; set; }
        [BindProperty] public ApplicationUserWithRoleBoolsViewModel BoolUser { get; set; }
        
        
        public AdminOverviewModel(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
        }
        
        public async Task OnGet()
        {
            ApplicationUserViewModels = _applicationUserDataService.GetAllUsers().Result;
            BoolUsers = new List<ApplicationUserWithRoleBoolsViewModel>();
            BoolUser = new ApplicationUserWithRoleBoolsViewModel(); //TODO: Implement after List of roles added to create user cmd
            
            foreach (var user in ApplicationUserViewModels.ApplicationUserViewModels)
            {
                BoolUsers.Add(new ApplicationUserWithRoleBoolsViewModel(user));
            }
        }

        public async Task<IActionResult> OnPostCreateAsync(CreateUserCommand createUserCommand)
        {
            var result = await _applicationUserDataService.CreateUser(createUserCommand);
            if (result.IsSuccesfull) Message = "User created. Check email for confirmation link";
            return Page();
        }

        public async Task OnPostUdateAsync(string OverviewName, string OverviewEmail, 
            bool OverviewIsAdmin, bool OverviewIsWarehouse, bool OverviewIsOffice, 
            bool OverviewIsClient, bool OverviewIsTransporter, string UserIdView)
        {
            var la = "";
        }

        public async Task OnPostDeleteAsync(string UserIdView)
        {
            var la = "";
        }

       /* public async Task<IActionResult> OnPostUpdateAsync(ApplicationUserViewModel LoggedInUser)
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
        }*/
    }
}