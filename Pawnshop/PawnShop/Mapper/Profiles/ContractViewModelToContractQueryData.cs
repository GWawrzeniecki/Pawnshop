using AutoMapper;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Modules.Contract.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class ContractViewModelToContractQueryData : Profile
    {
        public ContractViewModelToContractQueryData()
        {
            CreateMap<ContractViewModel, ContractQueryData>();
        }
    }
}