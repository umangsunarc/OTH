using AutoMapper;
using Wollo.Entities.Models;

namespace Wollo.API.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
            //Mapper.CreateMap<Movie, MovieViewModel>()
            //    .ForMember(vm => vm.Genre, map => map.MapFrom(m => m.Genre.Name))
            //    .ForMember(vm => vm.GenreId, map => map.MapFrom(m => m.Genre.ID))
            //    .ForMember(vm => vm.IsAvailable, map => map.MapFrom(m => m.Stocks.Any(s => s.IsAvailable)))
            //    .ForMember(vm => vm.NumberOfStocks, map => map.MapFrom(m => m.Stocks.Count))
            //    .ForMember(vm => vm.Image, map => map.MapFrom(m => string.IsNullOrEmpty(m.Image) == true ? "unknown.jpg" : m.Image));

            //Mapper.CreateMap<Genre, GenreViewModel>()
            //    .ForMember(vm => vm.NumberOfMovies, map => map.MapFrom(g => g.Movies.Count()));
            // code omitted
            //Mapper.CreateMap<Customer, CustomerViewModel>();

            //Mapper.CreateMap<Stock, StockViewModel>();

            Mapper.CreateMap<Transaction_History, Wollo.Entities.ViewModels.Transaction_History>();
        }
    }
}