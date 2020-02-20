using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.SuppliersList;

namespace LogisticsBooking.FrontEnd.DataServices.Models.Booking
{
    public class BookingViewModel : IHaveCustomMapping
    {

        public BookingViewModel()
        {
            SuppliersListViewModel = new SuppliersListViewModel();
            OrdersListViewModel = new List<OrderViewModel>();
        }
        public int ExternalId { get; set; }
        
      
        [Range( 1 , 33,  ErrorMessage = "Angiv venligst et tal mellem 1 - 33") ]
        public int TotalPallets { get; set; }
        public DateTime BookingTime { get; set; }
        public string TransporterName { get; set; }
        public int Port { get; set; }
        
        
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime ActualArrival { get; set; }
        
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime StartLoading { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public DateTime EndLoading { get; set; }
        
        
        public Guid InternalId { get; set; }
        public string Email { get; set; }
        
        public Guid TransporterId { get; set; }
        
        public int PalletsRemaining { get; set; }
        
        public SuppliersListViewModel SuppliersListViewModel { get; set; }
        
        public List<OrderViewModel> OrdersListViewModel { get; set; }
        
        public bool IsBookingAllowed { get; set; }
        
        public int PalletsCurrentlyOnBooking { get; set; }
        
        public IntervalViewModel Interval { get; set; }
        
        public bool IsValidated { get; set; }
        
        public void CreateMappings(Profile configuration)
        {
            

            configuration.CreateMap<BookingViewModel, CreateBookingCommand>()
                .ForMember(  dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.ExternalId))
                .ForMember(  dest => dest.DeliveryDate,
                    opt => opt.MapFrom(src => src.BookingTime))
                .ForMember(dest => dest.IntervalId , 
                    opt => opt.MapFrom(src => src.Interval.IntervalId))
                .ForMember(  dest => dest.TotalPallets,
                    opt => opt.MapFrom(src => src.TotalPallets))
                .ForMember(  dest => dest.TransporterId,
                    opt => opt.MapFrom(src => src.TransporterId))
                .ForMember(  dest => dest.CreateOrderCommand,
                    opt => opt.MapFrom(src => src.OrdersListViewModel))
                ;
        }
    }
}
