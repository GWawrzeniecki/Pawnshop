using AutoMapper;
using PawnShop.Core.SharedVariables;

namespace PawnShop.Mapper.Profiles
{
    public class UserSettingsProfile : Profile
    {
        public UserSettingsProfile()
        {
            CreateMap<IUserSettings, UserSettings>()
                .ReverseMap();
        }
    }
}