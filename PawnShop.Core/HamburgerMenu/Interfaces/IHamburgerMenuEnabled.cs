using PawnShop.Core.HamburgerMenu.Implementations;

namespace PawnShop.Core.HamburgerMenu.Interfaces
{
    public interface IHamburgerMenuEnabled
    {
        public HamburgerMenuItemBase HamburgerMenuItem { get; set; }

        public bool IsEnabled
        {
            get => HamburgerMenuItem.IsEnabled;
            set => HamburgerMenuItem.IsEnabled = value;
        }


    }
}