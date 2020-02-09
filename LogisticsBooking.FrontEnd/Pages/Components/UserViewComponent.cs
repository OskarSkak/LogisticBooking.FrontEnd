using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.ApplicationUser;
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
        {/*
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var id  = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "sub").Value;

            var user = await _applicationUserDataService.GetUserById(new GetUserByIdCommand
            {
                Id = Guid.Parse(id)
            });
            
            stopwatch.Stop();
            
            using (LogContext.PushProperty("X-Correlation-ID", HttpContext.TraceIdentifier))
            {
                _logger.LogWarning("UserViewComoponent took {time}ms" , stopwatch.ElapsedMilliseconds);
            }
*/
            return View(new ApplicationUserViewModel());
        }
    }
    
    
}