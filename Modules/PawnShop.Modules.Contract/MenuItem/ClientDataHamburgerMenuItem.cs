using MahApps.Metro.IconPacks;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using Prism.Commands;
using Prism.Regions;

namespace PawnShop.Modules.Contract.MenuItem
{
    public class ClientDataHamburgerMenuItem : HamburgerMenuItemBase, IRegionManagerAware
    {
        public override string DefaultNavigationPath => nameof(Views.ClientData);

        public IRegionManager RegionManager { get; set; }

        public ClientDataHamburgerMenuItem()
        {
            Command = new DelegateCommand(Navigate);
            Label = "Dane klienta";
            Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Account };
        }

        private void Navigate()
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, DefaultNavigationPath);
        }
    }
}