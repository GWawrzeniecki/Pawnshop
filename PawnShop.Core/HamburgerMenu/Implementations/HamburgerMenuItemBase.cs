using MahApps.Metro.Controls;
using IHamburgerMenuItemBase = PawnShop.Core.HamburgerMenu.Interfaces.IHamburgerMenuItemBase;

namespace PawnShop.Core.HamburgerMenu.Implementations
{
    public abstract class HamburgerMenuItemBase : HamburgerMenuIconItem, IHamburgerMenuItemBase
    {
        public abstract string DefaultNavigationPath { get; }
    }
}