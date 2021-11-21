using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Controls.SharedViews.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class SaleToSaleBasicInfoViewModel : Profile
    {
        public SaleToSaleBasicInfoViewModel()
        {
            CreateMap<Sale, SaleBasicInfoViewModel>()
                .ForMember(d => d.ContractItemName, opt => opt.MapFrom(s => s.ContractItem.Name))
                .ForMember(d => d.ContractItemDescription, opt => opt.MapFrom(s => s.ContractItem.Description))
                .ForMember(d => d.ContractItemEstimatedValue, opt => opt.MapFrom(s => s.ContractItem.EstimatedValue))
                .ForMember(d => d.ContractItemQuantity, opt => opt.MapFrom(s => s.ContractItem.Amount))
                .ForMember(d => d.ContractItemTechnicalCondition, opt => opt.MapFrom(s => s.ContractItem.TechnicalCondition))
                .ForMember(d => d.SelectedContractItemCategory, opt => opt.MapFrom(s => s.ContractItem.Category))
                .ForMember(d => d.SelectedContractItemUnitMeasure, opt => opt.MapFrom(s => s.ContractItem.Category.Measure));
        }
    }
}