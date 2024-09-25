using AutoMapper;
using RouteFinderAPI.Data.Models;
using Route = RouteFinderAPI.Data.Models.Route;

namespace RouteFinderAPI.Providers.Two.Mappings
{
    public class SearchProfile : Profile
    {
        public SearchProfile()
        {
            CreateMap<SearchRequest, Models.SearchRequest>()
                .ForMember(dest => dest.Departure, opt => opt.MapFrom(src => src.Origin))
                .ForMember(dest => dest.DepartureDate, opt => opt.MapFrom(src => src.OriginDateTime))
                .ForMember(dest => dest.Arrival, opt => opt.MapFrom(src => src.Destination))
                .ForMember(dest => dest.MinTimeLimit, opt => opt.MapFrom(src => src.Filters != null ? src.Filters.MinTimeLimit : null));

            CreateMap<Models.Route, Route>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                 .ForMember(dest => dest.Origin, opt => opt.MapFrom(src => src.Departure.NameOfPoint))
                 .ForMember(dest => dest.Destination, opt => opt.MapFrom(src => src.Arrival.NameOfPoint))
                 .ForMember(dest => dest.OriginDateTime, opt => opt.MapFrom(src => src.Departure.Date))
                 .ForMember(dest => dest.DestinationDateTime, opt => opt.MapFrom(src => src.Arrival.Date))
                 .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                 .ForMember(dest => dest.TimeLimit, opt => opt.MapFrom(src => src.TimeLimit));

            CreateMap<Models.SearchResponse, SearchResponse>()
                .ForMember(dest => dest.Routes, opt => opt.MapFrom(src => src.Routes))
                .ForMember(dest => dest.MinPrice, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Min(r => r.Price) : 0))
                .ForMember(dest => dest.MaxPrice, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Max(r => r.Price) : 0))
                .ForMember(dest => dest.MinMinutesRoute, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Min(r => (r.Arrival.Date - r.Departure.Date).TotalMinutes) : 0))
                .ForMember(dest => dest.MaxMinutesRoute, opt => opt.MapFrom(src => src.Routes.Any() ? src.Routes.Max(r => (r.Arrival.Date - r.Departure.Date).TotalMinutes) : 0));

        }
    }
}
