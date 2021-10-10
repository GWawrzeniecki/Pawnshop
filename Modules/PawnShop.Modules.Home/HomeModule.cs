﻿using PawnShop.Core.Privileges;
using PawnShop.Core.Regions;
using PawnShop.Modules.Home.MenuItem;
using PawnShop.Modules.Home.ViewModels;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace PawnShop.Modules.Home
{
    [Privilege("PawnShopTabs")]
    public class HomeModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public HomeModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.Regions[RegionNames.MenuRegion].Add(containerProvider.Resolve<HomeHamburgerMenuItem>());

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Views.Home, HomeViewModel>();
        }
    }
}