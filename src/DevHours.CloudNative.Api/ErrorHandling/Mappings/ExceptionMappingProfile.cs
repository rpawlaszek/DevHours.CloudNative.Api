using AutoMapper;
using System;
using System.Net;

namespace DevHours.CloudNative.Api.ErrorHandling.Mappings
{
    public class ExceptionMappingProfile : Profile
    {
        public ExceptionMappingProfile()
        {
            CreateMap<Exception, ExceptionResponse>()
                .ForMember(e => e.StatusCode, o => o.MapFrom(e => HttpStatusCode.BadRequest))
                .ForMember(e => e.Response, o => o.MapFrom(e => e.Message));

            CreateMap<NullReferenceException, ExceptionResponse>()
                .ForMember(e => e.StatusCode, o => o.MapFrom(e => HttpStatusCode.NotFound))
                .ForMember(e => e.Response, o => o.MapFrom(e => e.Message));
        }
    }
}