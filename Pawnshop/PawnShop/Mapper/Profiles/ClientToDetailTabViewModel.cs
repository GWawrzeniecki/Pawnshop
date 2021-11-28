using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Modules.Client.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class ClientToDetailTabViewModel : Profile
    {
        public ClientToDetailTabViewModel()
        {
            CreateMap<Client, DetailTabViewModel>()
                .ForPath(d => d.FullName,
                    opt => opt.MapFrom(s => $"{s.ClientNavigation.FirstName} {s.ClientNavigation.LastName}"))
                .ForPath(d => d.FullAddress,
                    opt => opt.MapFrom(s => $"{s.ClientNavigation.Address.Street} {s.ClientNavigation.Address.HouseNumber} {s.ClientNavigation.Address.ApartmentNumber ?? string.Empty}, {s.ClientNavigation.Address.PostCode}, {s.ClientNavigation.Address.City.City1}"))
                .ForPath(d => d.BirthDate, opt => opt.MapFrom(s => s.ClientNavigation.BirthDate))
                .ForPath(d => d.Pesel, opt => opt.MapFrom(s => s.Pesel))
                .ForPath(d => d.IdCardNumber, opt => opt.MapFrom(s => s.IdcardNumber));



        }
    }
}