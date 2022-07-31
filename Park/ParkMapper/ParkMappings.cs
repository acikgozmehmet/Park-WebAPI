using AutoMapper;
using Park.Models;
using Park.Models.Dtos;

namespace Park.ParkMapper
{
    public class ParkMappings :Profile
    {
        public ParkMappings()
        {

            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
        }
    }
}
