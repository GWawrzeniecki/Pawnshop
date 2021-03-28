using Prism.Regions;
using System.Windows;

namespace PawnShop.Services.Interfaces
{
    public interface IShellService
    {
        IRegionManager ShowShell<T>(string viewName) where T : Window;
    }
}