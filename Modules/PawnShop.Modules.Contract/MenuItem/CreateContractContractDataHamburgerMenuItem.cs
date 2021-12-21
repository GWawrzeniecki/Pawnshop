using MahApps.Metro.IconPacks;
using PawnShop.Core.HamburgerMenu.Implementations;
using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Modules.Contract.Views;
using Prism.Commands;
using Prism.Regions;

namespace PawnShop.Modules.Contract.MenuItem
{
    public class CreateContractContractDataHamburgerMenuItem : HamburgerMenuItemBase, IRegionManagerAware
    {
        public CreateContractContractDataHamburgerMenuItem()
        {
            Command = new DelegateCommand(Navigate);
            Label = "Dane umowy";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.FileDocumentEditOutline };

        }

        public override string DefaultNavigationPath => nameof(CreateContractContractData);

        public IRegionManager RegionManager { get; set; }

        private void Navigate()
        {
            RegionManager.RequestNavigate(RegionNames.ContentRegion, DefaultNavigationPath);
        }



    }
}