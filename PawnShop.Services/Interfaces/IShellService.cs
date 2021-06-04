using System.Windows;

namespace PawnShop.Services.Interfaces
{
    public interface IShellService
    {
        void ShowShell<T>(string viewName) where T : Window;
        void CloseShell<T>() where T : Window;
    }
}