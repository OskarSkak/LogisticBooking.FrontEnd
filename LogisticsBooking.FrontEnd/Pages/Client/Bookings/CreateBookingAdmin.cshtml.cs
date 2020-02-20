using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Transporter.Transporter;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
using LogisticsBooking.FrontEnd.Documents;
using LogisticsBooking.FrontEnd.Pages.Transporter.Booking;
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
        [BindProperty(SupportsGet = true)] public BookingViewModel Booking { get; set; }
        [BindProperty(SupportsGet = true)] public List<OrderViewModel> Orders { get; set; }
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
            Orders = new List<OrderViewModel>();
            for (int i = 0; i < 2; i++) Orders.Add(new OrderViewModel());
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var TransporterSupplierJoin = await transporterDataService.GetAllTransportersAndSuppliers();
            Transporters = TransporterSupplierJoin.Transporters;
            Suppliers = TransporterSupplierJoin.Suppliers;
            TransporterOptions = new SelectList(Transporters, nameof(TransporterViewModel.TransporterId), nameof(TransporterViewModel.Name));
            SupplierOptions = new SelectList(Suppliers, nameof(SupplierViewModel.SupplierId), nameof(SupplierViewModel.Name));
            Booking = new BookingViewModel();
            Booking.OrdersListViewModel = new List<OrderViewModel>();
            Booking.OrdersListViewModel.Add(new OrderViewModel());
            Booking.OrdersListViewModel.Add(new OrderViewModel());
            for (int i = 0; i < 2; i++) Orders.Add(new OrderViewModel());
            return Page();
        }


        public async Task<IActionResult> OnPostCreate(string TransporterId, BookingViewModel Booking)
        {
            Booking.TransporterId = Guid.Parse(TransporterId);
            var bookingNumber = await utilBookingDataService.GetBookingNumber();
            Booking.ExternalId = bookingNumber.bookingid;
            var i = 1;
            foreach (var order in Booking.OrdersListViewModel)
            {
                order.ExternalId = Booking.ExternalId + "-" + i++;
            }

            var id = User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
            
            HttpContext.Session.SetObject(id, Booking);
            
            return new RedirectToPageResult("select_timeAdmin");
            /*CreateBookingCommand = new CreateBookingCommand
            {
                DeliveryDate = Booking.BookingTime, 
                TotalPallets = Booking.TotalPallets,
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
            
            Booking.TransporterId = 

            var CreateCommandId = Guid.NewGuid();
            TempData.Set(CreateCommandId.ToString(), CreateBookingCommand);
            TempData.Set(CreateCommandId.ToString(), Booking);
            return new RedirectToPageResult("Error");*/
        }
    }
    
}

