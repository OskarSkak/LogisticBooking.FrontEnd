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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace LogisticsBooking.FrontEnd.Pages.Transporter.Booking
{
    public class orderinformation : PageModel 
    {
        private readonly ISupplierDataService _supplierDataService;
        private readonly IUtilBookingDataService _utilBookingDataService;


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
        
        public bool IsFirstOrder => !BookingViewModel.OrdersListViewModel.Any();
        

       

        public orderinformation(ISupplierDataService supplierDataService , IUtilBookingDataService utilBookingDataService)
        {
            _supplierDataService = supplierDataService;
            _utilBookingDataService = utilBookingDataService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await GenerateBookingViewModel();
            return Page();

        }

        public async Task<IActionResult> OnPostCreateOrderAsync(OrderViewModel orderViewModel )
        {

            // We have to remove totalpallets form the modelstate in order to check if the state is valid, because totalpallets is being carried over from last page
            ModelState.Remove("TotalPallets");
            if (!ModelState.IsValid)
            {
                var result = await GenerateBookingViewModel();
                if (!result)
                {
                    return new RedirectToPageResult("/Transporter/Booking/BookOrder");
                }
                ShowOrderMessage = true;
                OrderMessage = "Der var en fejl på ordren, klik på opret ordre for at se hvilke.";
                return Page();
            }
       
            // Gets currenr BookingModel from session
            BookingViewModel = GetBookingViewModelFromSession();

            // If it is first order, the supplier overlap check isnt neccesary'
            if (IsFirstOrder)
            {
                AddOrderToBookingViewModel(orderViewModel, BookingViewModel);
                IsBookingAllowed = true;
            }
            else
            {
                
                if (await CheckIfSupplierTimeOverlap(BookingViewModel, orderViewModel))
                {
                    AddOrderToBookingViewModel(orderViewModel, BookingViewModel);
                    IsBookingAllowed = true;
                }
                else
                {
                    // If the suppliers doesnt overlap, they cant be on the same booking, therefore set bookingAllowed to false and show error message
                    IsBookingAllowed = false;
                    SetBookingViewModelToSession(BookingViewModel);
                    Message = "Det er ikke muligt at booke de kundder på samme ordre";
                }
            }
            
            SetBookingViewModelToSession(BookingViewModel);
            return new RedirectToPageResult("orderinformation");

        }

        
        public IActionResult OnPostDelete(OrderViewModel orderViewModel)
        {
            var currentBookingViewModel = GetBookingViewModelFromSession();

            RemoveOrderViewModelFromBookingViewModel(currentBookingViewModel , orderViewModel.ExternalId);
            
            SetBookingViewModelToSession(currentBookingViewModel);
            
            
            // Has to decrease the current order id for the next order
            var currentOrderId = GetCurrentOrderId();
            SetCurrentOrderId(--currentOrderId);
            
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
        }
        
        
        /**
         * Method to check if two suplliers is in the same time range
         * return false if they are not in the same time range 
         */
        private async Task<bool> CheckIfSupplierTimeOverlap(BookingViewModel bookingViewModel , OrderViewModel orderViewModel)
        {
            var orderViewModelsInCurrentBooking = new List<SupplierViewModel>();
            
            foreach (var order in bookingViewModel.OrdersListViewModel)
            {
                orderViewModelsInCurrentBooking.Add(await _supplierDataService.GetSupplierById(order.SupplierViewModel.SupplierId));
            }
            
            var SupplierTryingToBook = await _supplierDataService.GetSupplierById(orderViewModel.SupplierViewModel.SupplierId);

            orderViewModelsInCurrentBooking.OrderBy(x => x.DeliveryStart);

            bool overlap = true;
            foreach (var supplier in orderViewModelsInCurrentBooking)
            {
                if (!Overlap(SupplierTryingToBook, supplier))
                {
                    overlap = false;
                    break;
                }
            }

            return overlap;
            
            
            
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
         * Return true if a supplier overlaps with another supplier
         */
        private bool Overlap(SupplierViewModel supplierViewModelTryingToBook,
            SupplierViewModel supplierViewModel)
        {
            TimeSpan start = supplierViewModel.DeliveryStart.TimeOfDay; // 10 PM
            TimeSpan end = supplierViewModel.DeliveryEnd.TimeOfDay;   // 2 AM
            TimeSpan start1 = supplierViewModelTryingToBook.DeliveryStart.TimeOfDay;
            TimeSpan end1 = supplierViewModelTryingToBook.DeliveryEnd.TimeOfDay;

            return TimeUtility.IsOverlapping(start, end, start1, end1);
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
        private void RemoveOrderViewModelFromBookingViewModel(BookingViewModel bookingViewModel, string orderId)
        {

            var orderViewModel = bookingViewModel.OrdersListViewModel.FirstOrDefault(x => x.ExternalId.Equals(orderId));

            bookingViewModel.OrdersListViewModel.Remove(orderViewModel);

        }

        /**
         * Iterate every order and counts total pallets, and updates the booking with correct numers.
         * The method is run on each refresh
         */
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
