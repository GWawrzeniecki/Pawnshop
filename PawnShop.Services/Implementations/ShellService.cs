using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Services.Interfaces;
using Prism.Ioc;
using Prism.Regions;
using System.Linq;
using System.Windows;

namespace PawnShop.Services.Implementations
{
    public class ShellService : IShellService
    {
        private readonly IContainerProvider _container;
        private readonly IRegionManager _regionManager;

        public ShellService(IContainerProvider container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void ShowShell<T>(string viewName) where T : Window
        {
            var scopedRegion = _regionManager.CreateRegionManager();
            var shell = _container.Resolve<T>((typeof(IRegionManager), scopedRegion), (typeof(IContainerProvider), _container));

            RegionManager.SetRegionManager(shell, scopedRegion);
            RegionManagerAware.SetRegionManagerAware(shell, scopedRegion);

            scopedRegion.RequestNavigate(RegionNames.ContentRegion, viewName);

            shell.Show();

        }

        public void CloseShell<T>() where T : Window
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window.GetType() == typeof(T))?.Close();
        }
    }
}