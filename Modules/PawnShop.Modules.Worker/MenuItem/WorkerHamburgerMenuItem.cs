using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;

namespace PawnShop.Modules.Worker.MenuItem
{
    public class WorkerHamburgerMenuItem : HamburgerMenuItemBase

    {
        #region private members

        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region public properties

        public override string DefaultNavigationPath => nameof(Views.Workers);

        #endregion public properties

        #region constructr

        public WorkerHamburgerMenuItem(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Pracownicy";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.AccountHardHat };
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