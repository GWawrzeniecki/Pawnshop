using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Services.DataService.InsertModels;

namespace PawnShop.Mapper.Profiles
{
    public class InsertContractItemToContractItemProfile : Profile
    {
        public InsertContractItemToContractItemProfile()
        {
            CreateMap<InsertContractItem, ContractItem>()
                .ReverseMap();
        }
    }
}