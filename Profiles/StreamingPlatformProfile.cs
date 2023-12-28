using AutoMapper;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Profiles
{
    public class StreamingPlatformProfile : Profile
    {
        public StreamingPlatformProfile()
        {
            CreateMap<CreateStreamingPlatformDto, StreamingPlatform>();
            CreateMap<UpdateStreamingPlatformDto, StreamingPlatform>();
            CreateMap<StreamingPlatform, UpdateStreamingPlatformDto>();
            CreateMap<StreamingPlatform, ReadStreamingPlatformDto>();
        }
    }
}
