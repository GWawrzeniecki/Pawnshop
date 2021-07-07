using Prism.Regions;
using System.Windows;

namespace PawnShop.Services.Interfaces
{
    public interface IShellService
    {
        void ShowShell<T>(string viewName, NavigationParameters navigationParameter = null) where T : Window;
        void CloseShell<T>() where T : Window;
        object GetShellViewModel<T>() where T : Window;
    }
}