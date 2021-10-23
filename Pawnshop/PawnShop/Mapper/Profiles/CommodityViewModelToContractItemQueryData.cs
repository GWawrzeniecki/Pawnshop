using AutoMapper;
using PawnShop.Core.Models.QueryDataModels;
using PawnShop.Modules.Commodity.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class CommodityViewModelToContractItemQueryData : Profile
    {
        public CommodityViewModelToContractItemQueryData()
        {
            CreateMap<CommodityViewModel, ContractItemQueryData>()
                .ForMember(d => d.ContractItemCategory, opt => opt.MapFrom(s => s.SelectedContractItemCategory))
                .ForMember(d => d.PriceOption, opt => opt.MapFrom(s => s.SelectedPriceOption))
                .ForMember(d => d.SalePrice, opt => opt.MapFrom(s => s.Price));
        }
    }
}