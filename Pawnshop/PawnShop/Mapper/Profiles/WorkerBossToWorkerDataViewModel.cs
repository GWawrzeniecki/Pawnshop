using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Modules.Worker.Dialogs.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class WorkerBossToWorkerDataViewModel : Profile
    {
        public WorkerBossToWorkerDataViewModel()
        {
            CreateMap<WorkerBoss, WorkerDataViewModel>()
                .ForMember(x => x.Salary, opt => opt.MapFrom(source => source.Salary))
                .ForMember(x => x.GrantedBonus, opt => opt.MapFrom(source => source.GrantedBonus))
                .ForMember(x => x.HireDate, opt => opt.MapFrom(source => source.HireDate))
                .ForMember(x => x.DatePhysicalCheckUp, opt => opt.MapFrom(source => source.DatePhysicalCheckUp))
                .ReverseMap();
        }
    }
}