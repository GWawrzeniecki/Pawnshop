using AutoMapper;
using PawnShop.Business.Models;
using PawnShop.Controls.ContractItemViews.ViewModels;

namespace PawnShop.Mapper.Profiles
{
    public class LaptopViewModelToLaptopProfile : Profile
    {

        public LaptopViewModelToLaptopProfile()
        {
            CreateMap<LaptopViewModel, Laptop>()
                .ReverseMap();
        }
    }
}
