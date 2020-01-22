using System;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.Interval.DetailInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels;

namespace LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels
{
    public class MasterIntervalStandardViewModel : IHaveCustomMapping
    {
        public Guid MasterIntervalStandardId { get; set;  }
        
        public DateTime? StartTime { get; set; }
        
        public DateTime? EndTime { get; set; }

        public int BottomPallets { get; set; }
        
        public Guid MasterScheduleStandardId { get; set; }
        
        public MasterScheduleStandardViewModel MasterScheduleStandardViewModel { get; set; }

        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MasterIntervalStandardViewModel, IntervalViewModel>()
                .ForMember(dest => dest.BottomPallets,
                    opt => opt.MapFrom(src => src.BottomPallets))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.IntervalId,
                    opt => opt.MapFrom(src => src.MasterIntervalStandardId))
                .ForMember(dest => dest.ScheduleId,
                    opt => opt.MapFrom(src => src.MasterScheduleStandardId));
            
        }
    }
}