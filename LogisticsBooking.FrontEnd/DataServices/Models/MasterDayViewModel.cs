using System;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;

namespace LogisticsBooking.FrontEnd.DataServices.Models
{
    public class MasterDayViewModel : IHaveCustomMapping
    {
        public Guid MasterDayId { get; set; }
        public DayOfWeek ActiveDay { get; set; }
        public bool IsActive { get; set; }
        
        public Guid MasterScheduleStandardId { get; set; }
        public MasterScheduleStandardViewModel MasterScheduleStandardViewModel { get; set; }

        public void CreateMappings(Profile configuration)
        {
            
            configuration.CreateMap<MasterDayViewModel, DayViewModel>()
                .ForMember(dest => dest.ActiveDay,
                    opt => opt.MapFrom(src => src.ActiveDay))
                .ForMember(dest => dest.DayId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.ScheduleViewModel,
                    opt => opt.MapFrom(src => src.MasterScheduleStandardViewModel));
        }
    }
}