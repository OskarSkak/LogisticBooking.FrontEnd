using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Documents;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace LogisticsBooking.FrontEnd.Pages.Client.Bookings
{
    public class CreateBookingAdminModel : PageModel
    {
        private IBookingDataService bookingDataService;
        private IInactiveBookingDataService inactiveBookingDataService;
        private ITransporterDataService transporterDataService;
        private IUtilBookingDataService utilBookingDataService;
        [BindProperty] public BookingViewModel Booking { get; set; }
        [BindProperty] public List<TransporterViewModel> Transporters { get; set; }
        [BindProperty] public List<SupplierViewModel> Suppliers { get; set; }
        public CreateBookingCommand CreateBookingCommand { get; set; }
        public SelectList TransporterOptions { get; set; }
        public SelectList SupplierOptions { get; set; }
        //TEMP
        //TODO: Create standard values for things like this (should be subject to change by user)
        public readonly int MAX_ORDERS = 2;
        
        
        public CreateBookingAdminModel(IBookingDataService _bookingDataService, IInactiveBookingDataService _inactiveBookingDataService, 
            ITransporterDataService _transporterDataService, IUtilBookingDataService _utilBookingDataService)
        {
            bookingDataService = _bookingDataService;
            inactiveBookingDataService = _inactiveBookingDataService;
            transporterDataService = _transporterDataService;
            utilBookingDataService = _utilBookingDataService;
            Booking = new BookingViewModel();
            Booking.OrdersListViewModel = new List<OrderViewModel>();
            Booking.OrdersListViewModel.Add(new OrderViewModel());
            Booking.OrdersListViewModel.Add(new OrderViewModel());
            foreach (var order in Booking.OrdersListViewModel) order.SupplierViewModel = new SupplierViewModel();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var TransporterSupplierJoin = await transporterDataService.GetAllTransportersAndSuppliers();
            Transporters = TransporterSupplierJoin.Transporters;
            Suppliers = TransporterSupplierJoin.Suppliers;
            TransporterOptions = new SelectList(Transporters, nameof(TransporterViewModel.TransporterId), nameof(TransporterViewModel.Name));
            SupplierOptions = new SelectList(Suppliers, nameof(SupplierViewModel.SupplierId), nameof(SupplierViewModel.Name));
            return Page();
        }


        public async Task<IActionResult> OnPostCreate(DateTime ViewBookingTime, string TransporterId, int ViewTotalPallets, BookingViewModel Booking)
        {
            var externalBookingId  = await utilBookingDataService.GetBookingNumber();

            CreateBookingCommand = new CreateBookingCommand
            {
                DeliveryDate = ViewBookingTime, 
                TotalPallets = ViewTotalPallets,
                TransporterId = Guid.Parse(TransporterId),
                CreateOrderCommand = new List<CreateOrderCommand>(), 
                ExternalId = externalBookingId.bookingid, 
                InOut = "Ind" //TODO: Change once added to bookings
            };

            var i = 1;

            foreach (var order in Booking.OrdersListViewModel)
            {
                if(order.BottomPallets != 0) CreateBookingCommand.CreateOrderCommand.Add(new CreateOrderCommand
                {
                    BottomPallets = order.BottomPallets, 
                    Comments = order.Comment, 
                    ExternalId = CreateBookingCommand.ExternalId + "-" + i++,
                    SupplierId = order.SupplierViewModel.SupplierId, 
                    TotalPallets = order.TotalPallets, 
                    OrderNumber = order.OrderNumber, 
                    InOut = order.InOut
                });
            }

            var CreateCommandId = Guid.NewGuid();
            TempData.Set(CreateCommandId.ToString(), CreateBookingCommand);
            var mike = TempData.Get<CreateBookingCommand>(CreateCommandId.ToString()); 
            return new RedirectToPageResult("Error");
        }
    }
    
}

