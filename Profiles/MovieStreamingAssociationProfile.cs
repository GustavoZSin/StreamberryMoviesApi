using AutoMapper;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Profiles
{
    public class MovieStreamingAssociationProfile : Profile
    {
        public MovieStreamingAssociationProfile()
        {
            CreateMap<CreateMovieStreamingAssociationDto, MovieStreamingAssociation>();
            CreateMap<MovieStreamingAssociation, ReadMovieStreamingAssociationDto>();
        }
    }
}
