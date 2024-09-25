using AutoMapper;
using RouteFinderAPI.Data.Models;
using Route = RouteFinderAPI.Data.Models.Route;

namespace RouteFinderAPI.Providers.One.Mappings
{
    public class SearchProfile : Profile
    {
        public SearchProfile()
        {

            CreateMap<SearchRequest, Models.SearchRequest>()
                .ForMember(dest => dest.From, opt => opt.MapFrom(src => src.Origin))
                .ForMember(dest => dest.To, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dest => dest.DateFrom, opt => opt.MapFrom(src => src.OriginDateTime))
                .ForMember(dest => dest.DateTo, opt => opt.MapFrom(src => src.Filters != null ? src.Filters.DestinationDateTime : null))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.Filters != null ? src.Filters.MaxPrice : null));

            CreateMap<Models.Route, Route>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.From))
                .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.To))
                .ForMember(dest => dest.OriginDateTime, opt => opt.MapFrom(src => src.DateFrom))
                .ForMember(dest => dest.DestinationDateTime, opt => opt.MapFrom(src => src.DateTo))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit));

            CreateMap<Models.SearchResponse, SearchResponse>()
                .ForMember(dest => dest.Routes, opt => opt.MapFrom(src => src.Routes))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Min(r => r.Price) : 0))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Max(r => r.Price) : 0))
                .ForMember(dest => dest.MinMinutesRoute, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Min(r => (r.DateTo - r.DateFrom).TotalMinutes) : 0))
                .ForMember(dest => dest.MaxMinutesRoute, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Max(r => (r.DateTo - r.DateFrom).TotalMinutes) : 0));
        }
    }
}
