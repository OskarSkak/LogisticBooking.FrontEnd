using System;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsBooking.FrontEnd.Pages.Components
{
    public class UserViewComponent : ViewComponent
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IApplicationUserDataService _applicationUserDataService;

        public UserViewComponent(ITransporterDataService transporterDataService , IApplicationUserDataService applicationUserDataService)
        {
            _transporterDataService = transporterDataService;
            _applicationUserDataService = applicationUserDataService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id  = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var user = await _applicationUserDataService.GetUserById(new GetUserByIdCommand
            {
                Id = Guid.Parse(id)
            });
            
            
            return View(user);
        }
    }
    
    
}