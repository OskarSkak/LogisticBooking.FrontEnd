using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace LogisticsBooking.FrontEnd.Pages.Components
{
    public class UserViewComponent : ViewComponent
    {
        private readonly ITransporterDataService _transporterDataService;
        private readonly IApplicationUserDataService _applicationUserDataService;
        private readonly ILogger<UserViewComponent> _logger;

        public UserViewComponent(ITransporterDataService transporterDataService , IApplicationUserDataService applicationUserDataService , ILogger<UserViewComponent> logger)
        {
            _transporterDataService = transporterDataService;
            _applicationUserDataService = applicationUserDataService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var id  = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            
            var currentUser = HttpContext.Session.GetObject<ApplicationUserViewModel>(id);
            if (currentUser == null)
            {
                var user = await _applicationUserDataService.GetUserById(new GetUserByIdCommand
                {
                    Id = Guid.Parse(id)
                });
                HttpContext.Session.SetObject(id, user);
                return View(user);
            }
            else if (currentUser.Name == null)
            {
                var user = await _applicationUserDataService.GetUserById(new GetUserByIdCommand
                {
                    Id = Guid.Parse(id)
                });
                HttpContext.Session.SetObject(id, user);
                return View(user);
            }

            return View(currentUser);
        }
    }
    
    
}