using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Modules.Worker.Dialogs.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class WorkerBossToPersonalDataViewModel : Profile
    {
        public WorkerBossToPersonalDataViewModel()
        {
            CreateMap<WorkerBoss, PersonalDataViewModel>()
                .ForMember(x => x.Pesel, opt => opt.MapFrom(source => source.Pesel))
                .ForPath(x => x.FirstName, opt => opt.MapFrom(source => source.WorkerBossNavigation.FirstName))
                .ForPath(x => x.LastName, opt => opt.MapFrom(source => source.WorkerBossNavigation.LastName))
                .ForPath(x => x.BirthDate, opt => opt.MapFrom(source => source.WorkerBossNavigation.BirthDate))
                .ForPath(x => x.PostCode, opt => opt.MapFrom(source => source.WorkerBossNavigation.Address.PostCode))
                .ForPath(x => x.Street, opt => opt.MapFrom(source => source.WorkerBossNavigation.Address.Street))
                .ForPath(x => x.ApartmentNumber,
                    opt => opt.MapFrom(source => source.WorkerBossNavigation.Address.ApartmentNumber))
                .ForPath(x => x.HouseNumber, opt => opt.MapFrom(source => source.WorkerBossNavigation.Address.HouseNumber))
                .ReverseMap();
        }
    }
}