using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;

namespace PawnShop.Modules.Contract.MenuItem
{
    public class ContractHamburgerMenuItem : HamburgerMenuItemBase
    {
        private readonly IApllicationCommands _applicationCommands;

        public override string DefaultNavigationPath => nameof(Views.Contract);

        public ContractHamburgerMenuItem(IApllicationCommands applicationCommands)
        {
            this._applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Umowy";
            Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.FileDocumentEditOutline };
        }

        private void Navigate()
        {
            _applicationCommands.NavigateCommand.Execute(DefaultNavigationPath);
        }
    }
}