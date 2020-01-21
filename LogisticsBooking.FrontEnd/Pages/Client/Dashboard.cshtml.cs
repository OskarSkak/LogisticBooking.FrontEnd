using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Dashboard;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule;
using LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailsList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;

namespace LogisticsBooking.FrontEnd.Pages.Client
{
    public class Dashboard : PageModel
    {
        private readonly IMasterScheduleDataService _masterScheduleDataService;
        private readonly ILogisticBookingApiDatabase _logisticBookingApiDatabase;
        private readonly IMapper _mapper;
        private readonly IDashboardDataService _dashboardDataService;


        [BindProperty]
        public MasterSchedulesStandardViewModel MasterSchedulesStandardViewModel { get; set; }

        [BindProperty]
        public DashboardViewModel DashboardViewModel { get; set; }

        public Dashboard( ILogisticBookingApiDatabase logisticBookingApiDatabase , IMapper mapper)
        {
           
            _logisticBookingApiDatabase = logisticBookingApiDatabase;
            _mapper = mapper;
        }
        
        public void OnGet()
        {

            /*
            var result = await GetActiveMasterSchedule();

            MasterSchedulesStandardViewModel = CreateMasterSchedules(result);

            DashboardViewModel = await GetDashboard();
            */
        }

        public async Task<MasterSchedulesStandardViewModel> GetActiveMasterSchedule()
        {
            var activeMasterSchedule =
                await _logisticBookingApiDatabase.MasterScheduleStandards.Include(e => e.MasterIntervalStandards).Where(e => e.IsActive).ToListAsync();
            

            return new MasterSchedulesStandardViewModel
            {
                MasterScheduleStandardViewModels = _mapper.Map<List<MasterScheduleStandardViewModel>>(activeMasterSchedule)
            };
        }


        public async Task<DashboardViewModel> GetDashboard()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            
            var bookings =
                _logisticBookingApiDatabase.Bookings.Include(e => e.Interval).Where(e => e.BookingTime.Value.Date == DateTime.Today.Date).ToList();
            dashboardViewModel.TotalBookings = bookings.Count;
            dashboardViewModel.BookingsLeft = bookings.Count;
            dashboardViewModel.TimeToNextDelivery = TimeSpan.FromHours(10000);
            foreach (var booking in bookings)
            {
                if (booking.Interval.EndTime.Value < DateTime.Now)
                {
                    dashboardViewModel.BookingsLeft--;
                }
                var timeToNextDelivery = booking.Interval.StartTime.Value.Subtract(DateTime.Now);
                
                if (dashboardViewModel.TimeToNextDelivery >= timeToNextDelivery && timeToNextDelivery.Ticks > 0)
                {
                    dashboardViewModel.TimeToNextDelivery = timeToNextDelivery;
                }

                
                
            }

            
            return dashboardViewModel;

        }

        private MasterSchedulesStandardViewModel CreateMasterSchedules(MasterSchedulesStandardViewModel masterSchedulesStandardViewModel)
        {
            List<MasterIntervalStandardViewModel> DayIntervale = new List<MasterIntervalStandardViewModel>();
            List<MasterIntervalStandardViewModel> NightInterval = new List<MasterIntervalStandardViewModel>();

            foreach (var masterScheduleStandardView in masterSchedulesStandardViewModel.MasterScheduleStandardViewModels)
            {
                if (masterScheduleStandardView.Shifts == Shift.Day)
                {
                    DayIntervale =
                        masterScheduleStandardView.MasterIntervalStandardViewModels.OrderBy(e => e.StartTime).ToList();

                    masterScheduleStandardView.MasterIntervalStandardViewModels = DayIntervale;
                }
                else
                {
                    NightInterval = masterScheduleStandardView.MasterIntervalStandardViewModels.OrderByDescending(e => e.StartTime)
                        .ToList();

                    masterScheduleStandardView.MasterIntervalStandardViewModels = NightInterval;
                }

                
            }

            return masterSchedulesStandardViewModel;
        }

        
    }
}