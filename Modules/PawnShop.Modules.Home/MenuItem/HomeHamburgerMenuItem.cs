using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;

namespace PawnShop.Modules.Home.MenuItem
{
    public class HomeHamburgerMenuItem : HamburgerMenuItemBase
    {
        #region private members

        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region public properties

        public override string DefaultNavigationPath => nameof(Views.Home);

        #endregion public properties

        #region constructr

        public HomeHamburgerMenuItem(IApplicationCommands applicationCommands)
        {
            this._applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = nameof(Views.Home);
            Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Home };
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