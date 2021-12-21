using MahApps.Metro.IconPacks;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Modules.Contract.Views;
using Prism.Commands;
using Prism.Regions;

namespace PawnShop.Modules.Contract.MenuItem
{
    public class CreateContractClientDataHamburgerMenuItem : HamburgerMenuItemBase, IRegionManagerAware
    {
        public CreateContractClientDataHamburgerMenuItem()
        {
            Command = new DelegateCommand(Navigate);
            Label = "Dane klienta";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Account };
        }

        public override string DefaultNavigationPath => nameof(CreateContractClientData);

        public IRegionManager RegionManager { get; set; }

        private void Navigate()
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, DefaultNavigationPath);
        }
    }
}