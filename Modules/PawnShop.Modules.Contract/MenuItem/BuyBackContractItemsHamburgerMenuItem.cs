using MahApps.Metro.IconPacks;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Modules.Contract.Views;
using Prism.Commands;
using Prism.Regions;

namespace PawnShop.Modules.Contract.MenuItem
{
    public class BuyBackContractItemsHamburgerMenuItem : HamburgerMenuItemBase, IRegionManagerAware
    {
        public BuyBackContractItemsHamburgerMenuItem()
        {
            Command = new DelegateCommand(Navigate);
            Label = "Lista towarow";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Basket };
        }

        public override string DefaultNavigationPath => nameof(BuyBackContractItems);

        public IRegionManager RegionManager { get; set; }

        private void Navigate()
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, DefaultNavigationPath);
        }
    }
}