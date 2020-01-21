using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.Supplier.Supplier;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class OrderViewModel : IHaveCustomMapping
    
    {
        
        public Guid OrderId { get; set; }
        public Guid BookingId { get; set; }
        

        
        public string Comment { get; set; }
        public string ExternalId { get; set; }
        
        public int CreatedOrders { get; set; }
        public string CustomerNumber { get; set; }
        
        [Required(ErrorMessage = "Kan ikke være tomt")]
        public string OrderNumber { get; set; }
        public int WareNumber { get; set; }
        public string InOut { get; set; }
        
        [Required(ErrorMessage = "Paller skal angives")]
        [Range(1 , 200 , ErrorMessage = "Skal være imellem 1 og 200") ]
        public int TotalPallets { get; set; }
        
        [Required(ErrorMessage = "Bundpaller skal angives")]
        [Range(1 , 200 , ErrorMessage = "Bundpaller skal være imellem 1 og 33") ]
        public int BottomPallets { get; set; }
        
        

        public SupplierViewModel SupplierViewModel {get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<OrderViewModel, CreateOrderCommand>()
                .ForMember(dest => dest.Comments,
                    opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.BottomPallets,
                    opt => opt.MapFrom(src => src.BottomPallets))
                .ForMember(dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.ExternalId))
                .ForMember(dest => dest.OrderNumber,
                    opt => opt.MapFrom(src => src.OrderNumber))
                .ForMember(dest => dest.SupplierId,
                    opt => opt.MapFrom(src => src.SupplierViewModel.SupplierId))
                .ForMember(dest => dest.TotalPallets,
                    opt => opt.MapFrom(src => src.TotalPallets))
                .ForMember(dest => dest.TotalPallets,
                    opt => opt.MapFrom(src => src.TotalPallets))
                .ForMember(dest => dest.InOut,
                    opt => opt.MapFrom(src => src.InOut));
        }
    }
}