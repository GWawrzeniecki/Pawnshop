using AutoMapper;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Modules.Client.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class ClientViewModelToClientQueryData : Profile
    {
        public ClientViewModelToClientQueryData()
        {
            CreateMap<ClientViewModel, ClientQueryData>();
        }
    }
}