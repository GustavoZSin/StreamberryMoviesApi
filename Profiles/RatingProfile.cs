using AutoMapper;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Profiles
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<CreateRatingDto, Rating>();
            CreateMap<UpdateRatingDto, Rating>();
            CreateMap<Rating, UpdateRatingDto>();
            CreateMap<Rating, ReadRatingDto>();
        }
    }
}
