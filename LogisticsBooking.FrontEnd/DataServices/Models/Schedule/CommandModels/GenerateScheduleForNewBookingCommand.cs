using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class GenerateScheduleForNewBookingCommand
    {
        public DateTime BookingTime { get; set; }
        public SuppliersListViewModel SupplierViewModels { get; set; }
    }
}