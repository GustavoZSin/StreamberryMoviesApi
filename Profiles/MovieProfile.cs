using AutoMapper;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<CreateMovieDto, Movie>();
            CreateMap<UpdateMovieDto, Movie>();
            CreateMap<Movie, UpdateMovieDto>();
            CreateMap<Movie, ReadMovieDto>()
                .ForMember(movieDto => movieDto.ServicesOfStreaming,
                    opt => opt.MapFrom(movie => movie.ServicesOfStreaming));
        }
    }
}
