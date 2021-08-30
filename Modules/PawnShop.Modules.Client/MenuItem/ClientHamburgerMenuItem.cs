﻿using MahApps.Metro.IconPacks;
using PawnShop.Core;
using PawnShop.Core.HamburgerMenu.Implementations;
using Prism.Commands;

namespace PawnShop.Modules.Client.MenuItem
{
    public class ClientHamburgerMenuItem : HamburgerMenuItemBase
    {
        #region private members

        private readonly IApplicationCommands _applicationCommands;

        #endregion private members

        #region public properties

        public override string DefaultNavigationPath => nameof(Views.Client);

        #endregion public properties

        #region constructr

        public ClientHamburgerMenuItem(IApplicationCommands applicationCommands)
        {
            this._applicationCommands = applicationCommands;
            Command = new DelegateCommand(Navigate);
            Label = "Klienci";
            Icon = new PackIconMaterial() { Kind = PackIconMaterialKind.Account };
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