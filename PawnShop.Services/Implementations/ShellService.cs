using PawnShop.Core.Regions;
using PawnShop.Core.ScopedRegion;
using PawnShop.Core.SharedVariables;
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
        private readonly ISessionContext _sessionContext;

        public ShellService(IContainerProvider container, IRegionManager regionManager, ISessionContext sessionContext)
        {
            _container = container;
            _regionManager = regionManager;
            _sessionContext = sessionContext;
        }

        public void ShowShell<T>(string viewName) where T : Window
        {
            var window = Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.GetType() == typeof(T));

            if (window != null)
            {
                window.Activate();
                return;
            }

            var scopedRegion = _regionManager.CreateRegionManager();
            var shell = _container.Resolve<T>((typeof(IRegionManager), scopedRegion), (typeof(IContainerProvider), _container));

            RegionManager.SetRegionManager(shell, scopedRegion);
            RegionManagerAware.SetRegionManagerAware(shell, scopedRegion);

            scopedRegion.RequestNavigate(RegionNames.ContentRegion, viewName);

            shell.Show();

        }

        public void CloseShell<T>() where T : Window
        {
            Application.Current.Windows.OfType<Window>().FirstOrDefault(window => window.IsActive && window.GetType() == typeof(T))?.Close();
        }


    }
}