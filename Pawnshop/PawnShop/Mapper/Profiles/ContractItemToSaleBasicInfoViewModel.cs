using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Controls.SharedViews.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class ContractItemToSaleBasicInfoViewModel : Profile
    {
        public ContractItemToSaleBasicInfoViewModel()
        {
            CreateMap<ContractItem, SaleBasicInfoViewModel>()
                .ForMember(d => d.ContractItemName, opt => opt.MapFrom(s => s.Name))
                .ForMember(d => d.ContractItemDescription, opt => opt.MapFrom(s => s.Description))
                .ForMember(d => d.ContractItemEstimatedValue, opt => opt.MapFrom(s => s.EstimatedValue))
                .ForMember(d => d.ContractItemQuantity, opt => opt.MapFrom(s => s.Amount))
                .ForMember(d => d.ContractItemTechnicalCondition, opt => opt.MapFrom(s => s.TechnicalCondition))
                .ForMember(d => d.SelectedContractItemCategory, opt => opt.MapFrom(s => s.Category))
                .ForMember(d => d.SelectedContractItemUnitMeasure, opt => opt.MapFrom(s => s.Category.Measure));


        }
    }
}