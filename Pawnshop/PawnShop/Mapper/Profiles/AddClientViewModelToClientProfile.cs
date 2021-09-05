using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Controls.Dialogs.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class AddClientViewModelToClientProfile : Profile
    {
        public AddClientViewModelToClientProfile()
        {
            CreateMap<AddClientDialogViewModel, Client>()
                .ForMember(x => x.Pesel, opt => opt.MapFrom(source => source.Pesel))
                .ForMember(x => x.IdcardNumber, opt => opt.MapFrom(source => source.IdCardNumber))
                .ForMember(x => x.ValidityDateIdcard, opt => opt.MapFrom(source => source.ValidityDateIdCard))
                .ForPath(x => x.ClientNavigation.FirstName, opt => opt.MapFrom(source => source.FirstName))
                .ForPath(x => x.ClientNavigation.LastName, opt => opt.MapFrom(source => source.LastName))
                .ForPath(x => x.ClientNavigation.BirthDate, opt => opt.MapFrom(source => source.BirthDate))
                .ForPath(x => x.ClientNavigation.Address.PostCode, opt => opt.MapFrom(source => source.PostCode))
                .ForPath(x => x.ClientNavigation.Address.Street, opt => opt.MapFrom(source => source.Street))
                .ForPath(x => x.ClientNavigation.Address.ApartmentNumber,
                    opt => opt.MapFrom(source => source.ApartmentNumber))
                .ForPath(x => x.ClientNavigation.Address.HouseNumber, opt => opt.MapFrom(source => source.HouseNumber))
                .ReverseMap();
        }
    }
}