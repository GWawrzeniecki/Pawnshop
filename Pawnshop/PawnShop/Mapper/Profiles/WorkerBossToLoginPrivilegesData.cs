using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Modules.Worker.Dialogs.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class WorkerBossToLoginPrivilegesData : Profile
    {
        public WorkerBossToLoginPrivilegesData()
        {
            CreateMap<WorkerBoss, LoginPrivilegesDataViewModel>()
                .ForMember(d => d.UserLogin, opt => opt.MapFrom(s => s.Login))
                .ForPath(d => d.BaseTabs, opt => opt.MapFrom(s => s.Privilege.PawnShopTabs))
                .ForPath(d => d.WorkerTab, opt => opt.MapFrom(s => s.Privilege.WorkersTab))
                .ForPath(d => d.SettingsTab, opt => opt.MapFrom(s => s.Privilege.SettingsTab))
                .ForMember(d => d.PasswordHash, opt => opt.MapFrom(src => src.Hash))
                .ReverseMap();
        }
    }
}