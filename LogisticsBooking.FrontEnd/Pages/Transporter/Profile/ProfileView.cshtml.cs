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

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Profile
{
    public class ProfileViewModel : PageModel
    {
        private readonly IApplicationUserDataService _applicationUserDataService;
        [BindProperty] public ApplicationUserViewModel LoggedInUser { get; set; }
        
        public ProfileViewModel(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
            LoggedInUser = new ApplicationUserViewModel();
        }
        
        public async Task OnGet()
        {
            //ApplicationUserViewModels = await _applicationUserDataService.GetAllUsers();
            var LoggedInIdString = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var LoggedInId = Guid.Parse(LoggedInIdString);
            LoggedInUser = await _applicationUserDataService.GetUserById(new GetUserByIdCommand{Id = LoggedInId});
            
        }

        public async Task<IActionResult> OnPostUpdateAsync(ApplicationUserViewModel LoggedInUser)
        {
            var LoggedInIdString = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
            var Roles = new List<string>();
            var cmd = new UpdateUserWithRolesCommand();
            cmd.ApplicationUserId = LoggedInIdString;
            Roles.Add("lager");
            if (!string.IsNullOrWhiteSpace(LoggedInUser.Name)) cmd.Name = LoggedInUser.Name;
            if (!string.IsNullOrWhiteSpace(LoggedInUser.Email)) cmd.Email = LoggedInUser.Email;

            var result = await _applicationUserDataService.UpdateUser(cmd); //TODO: Error handle
            return Page();
        }

        public async Task<IActionResult> OnPostPasswordAsync(string oldPass, string newPassConfirm, string newPass)
        {
            if (newPass != newPassConfirm || oldPass == null || newPass == null) return BadRequest();
            var LoggedInIdString = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            var cmd = new UpdatePasswordCommand
            {
                NewPass = newPass,
                OldPass = oldPass,
                UserId = LoggedInIdString
            };
            var result = await _applicationUserDataService.UpdatePassword(cmd);
            if (result.IsSuccesfull) return new RedirectToPageResult("ProfileView");
            return BadRequest();
        }
    }
}