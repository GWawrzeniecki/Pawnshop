using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Modules.Contract.Dialogs.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class AddContractItemDialogViewModelToContractItemProfile : Profile
    {
        public AddContractItemDialogViewModelToContractItemProfile()
        {
            CreateMap<AddContractItemDialogViewModel, ContractItem>()
                .ForMember(c => c.Name, opt => opt.MapFrom(source => source.ContractItemName))
                .ForMember(c => c.Amount, opt => opt.MapFrom(source => source.ContractItemQuantity))
                .ForMember(c => c.Category, opt => opt.MapFrom(source => source.SelectedContractItemCategory))
                .ForMember(c => c.CategoryId, opt => opt.MapFrom(source => source.SelectedContractItemCategory.Id))
                .ForMember(c => c.ContractItemState, opt => opt.MapFrom(source => source.SelectedContractItemState))
                .ForMember(c => c.ContractItemStateId, opt => opt.MapFrom(source => source.SelectedContractItemState.Id))
                .ForMember(c => c.Description, opt => opt.MapFrom(source => source.ContractItemDescription))
                .ForMember(c => c.EstimatedValue, opt => opt.MapFrom(source => source.ContractItemEstimatedValue))
                .ForMember(c => c.TechnicalCondition, opt => opt.MapFrom(source => source.ContractItemTechnicalCondition));
        }
    }
}