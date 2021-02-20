using PawnShop.Services.Interfaces;
using System.Windows.Input;

namespace PawnShop.Services.Implementations
{
    public class UIService : IUIService
    {
        public void ResetMouseCursor() => Mouse.OverrideCursor = null;

        public void SetMouseBusyCursor() => Mouse.OverrideCursor = Cursors.Wait;
    }
}