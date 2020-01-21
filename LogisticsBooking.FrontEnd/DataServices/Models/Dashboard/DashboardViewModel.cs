using System;
using System.Collections.Generic;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Dashboard
{
    public class DashboardViewModel 
    {
        public int TotalBookings { get; set; }

        public int BookingsLeft { get; set; }
        
        public List<IntervalViewModel> IntervalViewModels { get; set; }
        
        public TimeSpan TimeToNextDelivery { get; set; }
       
    }
}