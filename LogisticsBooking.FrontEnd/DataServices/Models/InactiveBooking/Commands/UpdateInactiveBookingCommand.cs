using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveOrder.ViewModels;

namespace LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.Commands
{
    public class UpdateInactiveBookingCommand
    {
        public Guid InternalId { get; set; }
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime? BookingTime { get; set; }
        public int Port { get; set; }
        public List<InactiveOrderViewModel> InactiveOrders { get; set; }

        public UpdateInactiveBookingCommand()
        {
            InactiveOrders = new List<InactiveOrderViewModel>();
        }

        public static UpdateInactiveBookingCommand GenerateCommand(InactiveBookingViewModel model)
        {
            var cmd = new UpdateInactiveBookingCommand
            {
                BookingTime = model.BookingTime,
                InternalId = model.InternalId,
                ExternalId = model.ExternalId, 
                InactiveOrders = model.InactiveOrders, 
                Port = model.Port, 
                TotalPallets = model.TotalPallets
            };

            return cmd;
        }
    }
}