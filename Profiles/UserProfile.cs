using AutoMapper;
using StreamberryMoviesApi.Data.Dtos;
using StreamberryMoviesApi.Models;

namespace StreamberryMoviesApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<User, UpdateUserDto>();
            CreateMap<User, ReadUserDto>();
        }
    }
}
