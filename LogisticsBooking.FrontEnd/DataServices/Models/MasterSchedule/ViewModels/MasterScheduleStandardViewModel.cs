using System;
using System.Collections.Generic;
using AutoMapper;
using LogisticsBooking.FrontEnd.Acquaintance.Interfaces;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterInterval.ViewModels;
using LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.Commands;
using Shift = LogisticsBooking.FrontEnd.DataServices.Models.Schedule.DetailSchedule.Shift;

namespace LogisticsBooking.FrontEnd.DataServices.Models.MasterSchedule.ViewModels
{
    public class MasterScheduleStandardViewModel : IHaveCustomMapping
    {
        public Guid MasterScheduleStandardId { get; set; }
        
        public Guid CreatedBy { get; set; }
        public int MischellaneousPallets { get; set; }
        public Shift Shifts { get; set; }
        public string Name { get; set; }
        
        public bool IsActive { get; set; }
        
        public List<MasterIntervalStandardViewModel> MasterIntervalStandardViewModels { get; set; }
        public void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<MasterScheduleStandardViewModel, CreateNewMasterScheduleStandardCommand>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Shifts,
                    opt => opt.MapFrom(src => src.Shifts))
                .ForMember(dest => dest.CreatedBy,
                    opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.MasterIntervalStandardViewModels,
                    opt => opt.MapFrom(src => src.MasterIntervalStandardViewModels))
                .ForMember(dest => dest.MischellaneousPallets,
                    opt => opt.MapFrom(src => src.MischellaneousPallets))
                .ForMember(dest => dest.IsActive,
                    opt => opt.MapFrom(src => src.IsActive));

            
            
        }
    }
}