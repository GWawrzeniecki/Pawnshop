using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;
using Prism.Events;

namespace PawnShop.Modules.Worker.MenuItem
{
    public class WorkerHamburgerMenuItem : ModuleHamburgerMenuItemBase

    {
        #region private members

        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region public properties

        public override string DefaultNavigationPath => nameof(Views.Workers);

        #endregion public properties

        #region constructr

        public WorkerHamburgerMenuItem(IApplicationCommands applicationCommands, IEventAggregator ea) : base("WorkerModule", ea)
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