using AutoMapper;
using DevHours.CloudNative.Api.Dtos;
using DevHours.CloudNative.Domain;

namespace DevHours.CloudNative.Api.Mappings
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            AllowNullCollections = true;
            CreateMap<RoomDto, Room>();
            CreateMap<Room, RoomDto>();
            CreateMap<BookingDto, Booking>();
            CreateMap<Booking, BookingDto>();
        }
    }
}