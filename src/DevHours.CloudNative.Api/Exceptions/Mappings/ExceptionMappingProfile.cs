using System;
using System.Net;
using AutoMapper;
using DevHours.CloudNative.Api.Exceptions;

namespace DevHours.CloudNative.Exceptions.Mappings
{
    public class ExceptionMappingProfile : Profile
    {
        public ExceptionMappingProfile()
        {
            CreateMap<Exception, ExceptionResponse>()
                .ForMember(e => e.StatusCode, o => o.MapFrom(e => HttpStatusCode.BadRequest))
                .ForMember(e => e.Response, o => o.MapFrom(e => e.Message));
        }
    }
}