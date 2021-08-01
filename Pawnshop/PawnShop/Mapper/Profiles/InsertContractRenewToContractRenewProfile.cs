using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Services.DataService.InsertModels;

namespace PawnShop.Mapper.Profiles
{
    public class InsertContractRenewToContractRenewProfile : Profile
    {
        public InsertContractRenewToContractRenewProfile()
        {
            CreateMap<InsertContractRenew, ContractRenew>();
        }
    }
}