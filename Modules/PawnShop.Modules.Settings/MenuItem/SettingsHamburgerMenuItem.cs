

using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;

namespace PawnShop.Modules.Settings.MenuItem
{
    public class SettingsHamburgerMenuItem : HamburgerMenuItemBase

    {
        #region private members

        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region public properties

        public override string DefaultNavigationPath => nameof(Views.Settings);

        #endregion public properties

        #region constructr

        public SettingsHamburgerMenuItem(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Ustawienia";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.CogOutline };
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