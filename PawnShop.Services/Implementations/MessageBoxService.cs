using BespokeFusion;
using PawnShop.Services.Interfaces;

namespace PawnShop.Services.Implementations
{
    public class MessageBoxService : IMessageBoxService
    {
        public void Show(string message, string title)
        {
            MaterialMessageBox.Show(message, title);
        }

        public void ShowError(string message, string title)
        {
            MaterialMessageBox.ShowError(message, title);
        }
    }
}