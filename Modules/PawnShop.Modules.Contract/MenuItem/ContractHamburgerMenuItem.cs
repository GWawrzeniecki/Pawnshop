using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;
using Prism.Events;

namespace PawnShop.Modules.Contract.MenuItem
{
    public class ContractHamburgerMenuItem : ModuleHamburgerMenuItemBase
    {
        private readonly IApplicationCommands _applicationCommands;

        public ContractHamburgerMenuItem(IApplicationCommands applicationCommands, IEventAggregator ea) : base("ContractModule", ea)
        {
            _applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Umowy";
            Icon = new PackIconMaterial { Kind = PackIconMaterialKind.FileDocumentEditOutline };
        }

        public override string DefaultNavigationPath => nameof(Views.Contract);

        private void Navigate()
        {
            _applicationCommands.NavigateCommand.Execute(DefaultNavigationPath);
        }
    }
}