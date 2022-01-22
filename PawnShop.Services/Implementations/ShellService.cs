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

        public void ShowShell<T>(string viewName, NavigationParameters navigationParameter = null) where T : Window
        {
            var window = Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.GetType() == typeof(T));

            if (window is not null)
            {
                window.Activate();
                return;
            }

            var scopedRegion = _regionManager.CreateRegionManager(); // creating scoped region manager
            var shell = _container.Resolve<T>((typeof(IRegionManager), scopedRegion), (typeof(IContainerProvider), _container)); // resolving shell

            RegionManager.SetRegionManager(shell, scopedRegion); // setting scoped region manager property for that shell
            RegionManagerAware.SetRegionManagerAware(shell, scopedRegion); // setting this so we can navigate in right region manager

            scopedRegion.RequestNavigate(RegionNames.ContentRegion, viewName, navigationParameter);

            shell.Show();

        }

        public void CloseShell<T>() where T : Window
        {
            Application.Current?.Windows.OfType<Window>().FirstOrDefault(window => window.GetType() == typeof(T))?.Close();
        }

        public object GetShellViewModel<T>() where T : Window
        {
            return Application.Current?.Windows.OfType<Window>()
                  .FirstOrDefault(window => window.GetType() == typeof(T))?.DataContext;
        }
    }
}