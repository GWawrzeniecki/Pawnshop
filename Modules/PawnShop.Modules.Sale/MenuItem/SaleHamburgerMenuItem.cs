using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;
using Prism.Events;

namespace PawnShop.Modules.Sale.MenuItem
{
    public class SaleHamburgerMenuItem : ModuleHamburgerMenuItemBase
    {
        #region private members

        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region public properties

        public override string DefaultNavigationPath => nameof(Views.Sale);

        #endregion public properties

        #region constructr

        public SaleHamburgerMenuItem(IApplicationCommands applicationCommands, IEventAggregator ea) : base("SaleModule", ea)
        {
            _applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Sprzedaż";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.Sale };
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