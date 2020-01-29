using System;
using System.Collections.Generic;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.InactiveOrder.ViewModels;

namespace LogisticsBooking.FrontEnd.DataServices.Models.InactiveBooking.Commands
{
    public class UpdateInactiveBookingWithOrdersCommand
    {
        public Guid InternalId { get; set; }
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime? BookingTime { get; set; }
        public int Port { get; set; }
        public List<InactiveOrderViewModel> InactiveOrders { get; set; }

        public UpdateInactiveBookingWithOrdersCommand()
        {
            InactiveOrders = new List<InactiveOrderViewModel>();
        }

        public static UpdateInactiveBookingWithOrdersCommand GenerateCommand(InactiveBookingViewModel model)
        {
            var cmd = new UpdateInactiveBookingWithOrdersCommand
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