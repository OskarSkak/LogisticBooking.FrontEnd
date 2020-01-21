using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LogisticsBooking.FrontEnd.Pages.Client.admin
{
    public class UserSingle : PageModel
    {
        private readonly IApplicationUserDataService _applicationUserDataService;

        public ApplicationUserViewModel ApplicationUserViewModel { get; set; }
        
        [BindProperty] 
        public List<SelectListItem> ActiveRoles { get; set; }
        
        [BindProperty] 
        public List<SelectListItem> InactiveRoles { get; set; }
        
        [BindProperty]
        public string SelectedActiveRole { get; set; }
        [BindProperty]
        public string SelectedInactiveRole { get; set; }



        public UserSingle(IApplicationUserDataService applicationUserDataService)
        {
            _applicationUserDataService = applicationUserDataService;
        }

        public async Task OnGet(Guid id)
        {
            ApplicationUserViewModel = await _applicationUserDataService.GetUserById(new GetUserByIdCommand
            {
                Id = id
            });

            ActiveRoles = CreateSelectList(ApplicationUserViewModel.ActiveRoles);
            InactiveRoles = CreateSelectList(ApplicationUserViewModel.InactiveRoles);

        }


        public async Task<IActionResult> OnPostUpdateUser(ApplicationUserViewModel applicationUserViewModel )
        {
            var response = await _applicationUserDataService.UpdateUser(applicationUserViewModel);
            
            return new RedirectToPageResult("UserSingle" , new {id = applicationUserViewModel.ApplicationUserId}); 
        }


        public async Task<IActionResult> OnPostUpAsync(string SelectedActiveRole, string SelectedInactiveRole , ApplicationUserViewModel applicationUserViewModel)
        {
          var result =  await _applicationUserDataService.UpdateUserRole(new UpdateRoleCommand
            {
                ApplicationUserId = Guid.Parse(applicationUserViewModel.ApplicationUserId),
                RoleToAdd = SelectedInactiveRole
            });
            
          
            return new RedirectToPageResult("UserSingle" ,new {id =applicationUserViewModel.ApplicationUserId});
        }
        
        public async Task<IActionResult> OnPostDownAsync(string SelectedActiveRole, string SelectedInactiveRole)
        {
            Console.WriteLine();
            
            return new RedirectToPageResult("UserSingle" ,new {id =""});
        }

        public async Task<IActionResult> OnPostDeleteAsync(ApplicationUserViewModel applicationUserViewModel)
        {
            var result = await _applicationUserDataService.DeleteUser(Guid.Parse(applicationUserViewModel.ApplicationUserId));

            if (result.IsSuccesfull)
            {
                return new RedirectResult("AllUsers");
            }

            return BadRequest();
        }

        
        public List<SelectListItem> CreateSelectList(List<IdentityRole> identityRoles)
        {
            List<SelectListItem> roles = new List<SelectListItem>();

            foreach (var role in identityRoles)
            {
                roles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Name
                });
            }

            return roles;
        }
    }
}