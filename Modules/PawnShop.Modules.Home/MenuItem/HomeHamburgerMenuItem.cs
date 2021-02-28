using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Modules.Home.MenuItem
{
    public class HomeHamburgerMenuItem : HamburgerMenuItemBase
    {
        public override string DefaultNavigationPath => "Home";


        private readonly IApllicationCommands applicationCommands;


        public HomeHamburgerMenuItem(IApllicationCommands applicationCommands)
        {
            this.applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Home";
            Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Home };
        }

        private void Navigate()
        {
            applicationCommands.NavigateCommand.Execute(DefaultNavigationPath);
        }
    }
}
