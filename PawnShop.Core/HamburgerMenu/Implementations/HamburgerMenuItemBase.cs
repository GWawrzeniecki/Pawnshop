namespace PawnShop.Core.HamburgerMenu.Implementations
{
    public abstract class HamburgerMenuItemBase : HamburgerMenuIconItem, Interfaces.IHamburgerMenuItemBase
    {
        public abstract string DefaultNavigationPath { get; }
    }
}