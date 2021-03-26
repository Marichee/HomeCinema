using AutoMapper;
using HomeCinema.Domain;
using HomeCinema.Domain.Models;
using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.Services.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Movie, MovieViewModel>()

                .ReverseMap()
                .ForMember(m => m.UserActionMovies, src => src.Ignore());
                
            CreateMap<User, UserViewModel>();
            CreateMap<RegisterViewModel, User>()
                .ForMember(r => r.FullName, src => src.ResolveUsing(rm => $"{rm.FirstName} {rm.LastName}"))
                .ForMember(r => r.EmailConfirmed, src => src.UseValue(true));
            CreateMap<UserActions, UserActionsViewModel>()
                .ForMember(a => a.Movies, src => src.Ignore())
                .ForMember(a=>a.Movies,src=>src.MapFrom(ua=>ua.UserActionMovies))
                .ReverseMap()
                .ForMember(ua => ua.UserActionMovies, src => src.Ignore())
                .ForMember(ua=>ua.UserId,src=>src.MapFrom(a=>a.User.Id))
                .ForMember(ua=>ua.User,src=>src.Ignore());
            CreateMap<UserActionMovies, MovieViewModel>()
                .ForMember(mv => mv.Id, src => src.MapFrom(ua => ua.Movie.Id))
                .ForMember(mv => mv.Name, src => src.MapFrom(ua => ua.Movie.Name))
                .ForMember(mv => mv.Genre, src => src.MapFrom(ua => ua.Movie.Genre))
                .ForMember(mv=>mv.OriginalId,src=>src.MapFrom(ua=>ua.Movie.OriginalId));

        }
    }
}
