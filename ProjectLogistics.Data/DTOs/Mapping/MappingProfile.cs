using AutoMapper;
using ProjectLogistics.Data.Entities;

namespace ProjectLogistics.Data.DTOs.Mapping
{
    /// <summary>
    /// To be implemented
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WarehouseDTO, Warehouse>()
                .ForMember(m => m.Id, p => p.MapFrom(s => s.Id))
                .ForMember(m => m.Name, p => p.MapFrom(s => s.Name))
                .ForMember(m => m.Latitude, p => p.MapFrom(s => s.Latitude))
                .ForMember(m => m.Longitude, p => p.MapFrom(s => s.Longitude));
        }
    }
}
