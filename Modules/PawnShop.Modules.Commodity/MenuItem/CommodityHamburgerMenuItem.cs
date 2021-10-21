using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;
using Prism.Events;

namespace PawnShop.Modules.Commodity.MenuItem
{
    public class CommodityHamburgerMenuItem : ModuleHamburgerMenuItemBase
    {
        #region private members

        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region public properties

        public override string DefaultNavigationPath => nameof(Views.Commodity);

        #endregion public properties

        #region constructr

        public CommodityHamburgerMenuItem(IApplicationCommands applicationCommands, IEventAggregator ea) : base("CommodityModule", ea)
        {
            _applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Towar";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Warehouse };
        }

        #endregion constructr

        #region private methods

        private void Navigate()
        {
            _applicationCommands.NavigateCommand.Execute(DefaultNavigationPath);
        }

        #endregion private methods
    }
}