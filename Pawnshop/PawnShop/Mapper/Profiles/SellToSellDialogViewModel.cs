using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Modules.Sale.Dialogs.ViewModels;
using System.Collections.Generic;

namespace PawnShop.Mapper.Profiles
{
    public class SellToSellDialogViewModel : Profile
    {
        public SellToSellDialogViewModel()
        {
            CreateMap<Sale, SellDialogViewModel>()
                .ForPath(d => d.ItemName, opt => opt.MapFrom(s => s.ContractItem.Name))
                .ForPath(d => d.BoughtPrice, opt => opt.MapFrom(s => s.ContractItem.EstimatedValue))
                .ForPath(d => d.Shelf, opt => opt.MapFrom(s => s.LocalSale.Shelf))
                .ForPath(d => d.Rack, opt => opt.MapFrom(s => s.LocalSale.Rack))
                .ForPath(d => d.SaleLinks, opt => opt.MapFrom(s => s.Links))
                .ForPath(d => d.SellPrice, opt => opt.MapFrom(s => s.SalePrice))
                .ForPath(d => d.ItemQuantity, opt => opt.MapFrom(s => s.ContractItem.Amount))
                .ForPath(d => d.SellItemAmount, opt => opt.MapFrom(s => s.Quantity))
                .ForPath(d => d.SoldPrice, opt => opt.MapFrom(s => s.SalePrice))
                .ForPath(d => d.ContractItemUnitMeasures, opt => opt.MapFrom(s => new List<UnitMeasure>() { s.ContractItem.Category.Measure }))
                .ForPath(d => d.SelectedContractItemUnitMeasure,
                    opt => opt.MapFrom(s => s.ContractItem.Category.Measure))
                .ForPath(d => d.SelectedContractItemUnitMeasureForSale,
                    opt => opt.MapFrom(s => s.ContractItem.Category.Measure));








        }
    }
}