using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;

namespace LogisticsBooking.FrontEnd.DataServices.Models.DeletedBooking.ViewModels
{
    public class DeletedBookingViewModel
    {
        public SuppliersListViewModel SuppliersListViewModel { get; set; }
        public List<OrderViewModel> OrdersListViewModel { get; set; }
        public Guid InternalId { get; set; }
        public int ExternalId { get; set; }
        public int TotalPallets { get; set; }
        public DateTime? BookingTime { get; set; }
        public int Port { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime? ActualArrival { get; set; }
       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime? StartLoading { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime? EndLoading { get; set; }
        public string Email { get; set; } 
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime? StartTime { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime? EndTime { get; set; }
        public int BottomPallets { get; set; }
        public Guid TransporterId { get; set; }
        public string TransporterName { get; set; }

        public DeletedBookingViewModel()
        {
            SuppliersListViewModel = new SuppliersListViewModel();
            OrdersListViewModel = new List<OrderViewModel>();
        }
        
    }
}