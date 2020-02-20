using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices.Models;
using LogisticsBooking.FrontEnd.DataServices.Models.Booking;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;
using LogisticsBooking.FrontEnd.DataServices.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class orderinformation : PageModel 
    {
        
        /*************************************************************** PROPERTIES **************************************************************/
        
        
        [BindProperty]
        public BookingViewModel BookingViewModel { get; set; }
        
        [BindProperty]
        public OrderViewModel OrderViewModel { get; set; }
        
        [BindProperty]
        public List<SelectListItem> Suppliers { get; set;}

        [BindProperty]
        public bool IsBookingAllowed { get; set; }
        
        [TempData]
        public string Message { get; set; }
        
        [TempData]
        public string OrderMessage { get; set; }
        
        public bool ShowMessage => !String.IsNullOrEmpty(Message);

        public bool ShowOrderMessage { get; set; }
        
        private bool IsFirstOrder => !BookingViewModel.OrdersListViewModel.Any();
        
        
        private readonly ISupplierDataService _supplierDataService;
        private readonly IUtilBookingDataService _utilBookingDataService;
        private readonly IBookingValidationDataService _bookingValidationDataService;

        
        /*************************************************************** CONSTRUCTOR **************************************************************/
        
        public orderinformation(ISupplierDataService supplierDataService , IUtilBookingDataService utilBookingDataService , IBookingValidationDataService bookingValidationDataService)
        {
            _supplierDataService = supplierDataService;
            _utilBookingDataService = utilBookingDataService;
            _bookingValidationDataService = bookingValidationDataService;
        }
        
        /*************************************************************** CONTROLLER METHODS **************************************************************/
        
        public async Task<IActionResult> OnGetAsync()
        {
            await GenerateBookingViewModel();
            return Page();

        }

        /**
         * When a user adds a order to the booking this controller is called
         */
        public async Task<IActionResult> OnPostCreateOrderAsync(OrderViewModel orderViewModel )
        {
            if (!await ModelIsValid())
            {
                return Page();  
            }
            
            BookingViewModel = GetBookingViewModelFromSession();
            
            
            if (IsFirstOrder)
            {
                AddOrderToBookingViewModel(orderViewModel, BookingViewModel);
            }
            else
            {
               await AddOrderToBookingViewModelWithCheck(orderViewModel , BookingViewModel);
               
            }
            
            
            SetBookingViewModelToSession(BookingViewModel);
            return new RedirectToPageResult("orderinformation");

        }
        
        public IActionResult OnPostDelete(OrderViewModel orderViewModel)
        {
            RemoveOrderViewModelFromBookingViewModel(orderViewModel.ExternalId);
            return new RedirectToPageResult("");
        }
        
        
        public IActionResult OnPostEditOrder(OrderViewModel orderViewModel , string comment)
        {
            ModelState.Remove("TotalPallets");

            if (!ModelState.IsValid)
            {
                
                return new RedirectToPageResult("");
            }

            orderViewModel.Comment = comment;
            var currentBookingViewModel = GetBookingViewModelFromSession();
            
            
            EditOrderViewModel(currentBookingViewModel , orderViewModel );
            
            SetBookingViewModelToSession(currentBookingViewModel);

            return new RedirectToPageResult("");
  
        }

        
        
        /*************************************************************** INTERNAL METHODS **************************************************************/

        private async Task<bool> ModelIsValid()
        {
            ModelState.Remove("TotalPallets");
            if (!ModelState.IsValid)
            {
                var result = await GenerateBookingViewModel();
                if (!result)
                {
                    return false;
                }
                ShowOrderMessage = true;
                OrderMessage = "Der var en fejl på ordren, klik på opret ordre for at se hvilke.";

                return false;
            }

            return true;
        }

        private async Task AddOrderToBookingViewModelWithCheck(OrderViewModel orderViewModel, BookingViewModel bookingViewModel)
        {
            if (await IfSupplierTimeOverlap(BookingViewModel, orderViewModel))
            {
                AddOrderToBookingViewModel(orderViewModel, BookingViewModel);
            }
            else
            {
                // If the suppliers doesnt overlap, they cant be on the same booking, therefore set bookingAllowed to false and show error message
                IsBookingAllowed = false;
                SetBookingViewModelToSession(BookingViewModel);
                Message = "Det er ikke muligt at booke de kundder på samme ordre";
            }
        }
        
        
        /**
         * Add a order to the booking
         */
        private void AddOrderToBookingViewModel(OrderViewModel orderViewModel, BookingViewModel bookingViewModel)
        {
            // Gets the order ID from the context
            var orderId = GetCurrentOrderId();
            
            
            var supplier = bookingViewModel.SuppliersListViewModel.Suppliers.FirstOrDefault(x =>
                x.SupplierId.Equals(orderViewModel.SupplierViewModel.SupplierId));



            bookingViewModel.OrdersListViewModel.Add(new OrderViewModel
            {
                OrderNumber = orderViewModel.OrderNumber,
                BottomPallets = orderViewModel.BottomPallets,
                CustomerNumber = orderViewModel.CustomerNumber,
                InOut = orderViewModel.InOut,
                TotalPallets = orderViewModel.TotalPallets,
                WareNumber = orderViewModel.WareNumber,
                SupplierViewModel = supplier,
                ExternalId = bookingViewModel.ExternalId + "-" + orderId.ToString("D2"),
                Comment = orderViewModel.Comment
               

            });
            // Has to increment the orderId for the next order
            SetCurrentOrderId(++orderId);
            IsBookingAllowed = true;
        }
        
        
        /**
         * Method to check if two suplliers is in the same time range
         * return false if they are not in the same time range 
         */
        private async Task<bool> IfSupplierTimeOverlap(BookingViewModel bookingViewModel , OrderViewModel orderViewModel)
        {

            var response = await _bookingValidationDataService.CheckIfSuppliersOverlap(
                new BookingSuppliersOverlapValidationCommand
                    {BookingViewModel = bookingViewModel, OrderViewModel = orderViewModel});

            return response.IsSuccess;
        }



        private BookingViewModel GetBookingViewModelFromSession()
        {
            // Gets the current booking from the session.
            return HttpContext.Session.GetObject<BookingViewModel>(GetLoggedInUserId());
        }

        private void SetBookingViewModelToSession(BookingViewModel bookingViewModel)
        {
            HttpContext.Session.SetObject(GetLoggedInUserId() , bookingViewModel);
        }
        
        /**
         * gets the current logged in user
         */
        private string GetLoggedInUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == "sub").Value;
        }
        
        
        
        private void CreateSelectedList(SuppliersListViewModel suppliers) 
        {
            Suppliers = new List<SelectListItem>();

            foreach (var supplier in suppliers.Suppliers)
            {
                Suppliers.Add(new SelectListItem{ Value = supplier.SupplierId.ToString() ,Text = supplier.Name});
            }
        }

        
        /**
         * Gets the current order id from the session
         */
        private int GetCurrentOrderId()
        {
            return HttpContext.Session.GetObject<int>(BookingViewModel.ExternalId.ToString());
        }

        
        /**
         * The method sets the current Order id in the session.
         * The Id has to be incremented when a order is added
         */
        private void SetCurrentOrderId(int currentOrderId)
        {
            HttpContext.Session.SetObject(BookingViewModel.ExternalId.ToString() , currentOrderId);
        }

        
        /**
         * The method updates a order on the booking. 
         */
        private void EditOrderViewModel(BookingViewModel bookingViewModel, OrderViewModel orderViewModel)
        {
            
            var order = bookingViewModel.OrdersListViewModel.Find(x => x.ExternalId.Equals(orderViewModel.ExternalId));
            

            order.Comment = orderViewModel.Comment;
            order.OrderNumber = orderViewModel.OrderNumber;
            order.TotalPallets = orderViewModel.TotalPallets;
            order.BottomPallets = orderViewModel.BottomPallets;
            order.InOut = orderViewModel.InOut;
            
        }
        

        /**
         * The method removes a order from the booking with the specific ID
         */
        private void RemoveOrderViewModelFromBookingViewModel( string orderId)
        {
            var currentBookingViewModel = GetBookingViewModelFromSession();

            var orderViewModel = currentBookingViewModel.OrdersListViewModel.FirstOrDefault(x => x.ExternalId.Equals(orderId));

            currentBookingViewModel.OrdersListViewModel.Remove(orderViewModel);
            
            SetBookingViewModelToSession(currentBookingViewModel);
            // Has to decrease the current order id for the next order
            
            UpdateOrderId();
            
            

        }

        private void UpdateOrderId()
        {
            var currentOrderId = GetCurrentOrderId();
            SetCurrentOrderId(--currentOrderId);
        }


        private void UpdateTotalPallets(BookingViewModel bookingViewModel)
        {
            
            int totalBottomPallets = 0;
            if (bookingViewModel.OrdersListViewModel != null)
            {
                foreach (var order in bookingViewModel.OrdersListViewModel)
                {
                    totalBottomPallets += order.BottomPallets;
                }
            }
            

            bookingViewModel.PalletsCurrentlyOnBooking = totalBottomPallets;

            bookingViewModel.PalletsRemaining = BookingViewModel.TotalPallets - totalBottomPallets;
            
            
        }

        /**
         * Sets the Viewmodels so they can be used in the view
         */
        private async Task<bool> GenerateBookingViewModel()
        {
             
            BookingViewModel = GetBookingViewModelFromSession();
            if (!BookingViewModel.SuppliersListViewModel.Suppliers.Any())
            {
                var id = GetLoggedInUserId();
                var result = await _supplierDataService.GetSupplierByTransporter(Guid.Parse(id));
                if (result == null)
                {
                    return false;
                }

                BookingViewModel.SuppliersListViewModel = result;
            }
            
            CreateSelectedList(BookingViewModel.SuppliersListViewModel);
            
            
            UpdateTotalPallets(BookingViewModel);
            SetBookingViewModelToSession(BookingViewModel);

            return true;
        }
        
    }
}
