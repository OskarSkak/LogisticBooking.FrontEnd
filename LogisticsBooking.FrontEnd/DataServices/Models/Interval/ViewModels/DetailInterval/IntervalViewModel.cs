using System;
using System.Collections.Generic;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval
{
    public class IntervalViewModel : IHaveCustomMapping
    {
        public Guid IntervalId { get; set;  }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public bool IsBooked { get; set; }
        public int BottomPallets { get; set; }
        public int RemainingPallets { get; set; }
        
        
        
        // ************************************* Relations ******************************

        
        public Guid ScheduleId { get; set; }

        public List<BookingViewModel> Bookings { get; set; }


        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<IntervalViewModel, MasterIntervalStandardViewModel>();
        }
    }
}
