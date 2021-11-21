using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Controls.SharedViews.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class SaleToSaleInfoViewModel : Profile
    {
        public SaleToSaleInfoViewModel()
        {
            CreateMap<Sale, SaleInfoViewModel>()

                .ForMember(d => d.ContractItemQuantity, opt => opt.MapFrom(s => s.ContractItem.Amount))
                .ForMember(d => d.SelectedContractItemUnitMeasure, opt => opt.MapFrom(s => s.ContractItem.Category.Measure))
                .ForMember(d => d.PutOnSaleDate, opt => opt.MapFrom(s => s.PutOnSaleDate))
                .ForMember(d => d.Price, opt => opt.MapFrom(s => s.SalePrice))
                .ForMember(d => d.Shelf, opt => opt.MapFrom(s => s.LocalSale.Shelf))
                .ForMember(d => d.Rack, opt => opt.MapFrom(s => s.LocalSale.Rack))
                .ForMember(d => d.SaleLinks, opt => opt.MapFrom(s => s.Links));



        }
    }
}