using MahApps.Metro.IconPacks;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Modules.Contract.Views;
using Prism.Commands;
using Prism.Regions;

namespace PawnShop.Modules.Contract.MenuItem
{
    public class SummaryHamburgerMenuItem : HamburgerMenuItemBase, IRegionManagerAware
    {
        public SummaryHamburgerMenuItem()
        {
            Command = new DelegateCommand(Navigate);
            Label = "Podsumowanie";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.CreditCard };

        }

        public override string DefaultNavigationPath => nameof(Summary);

        public IRegionManager RegionManager { get; set; }

        private void Navigate()
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, DefaultNavigationPath);
        }
    }
}