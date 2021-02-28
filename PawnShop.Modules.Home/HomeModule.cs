﻿using PawnShop.Core.Regions;
using PawnShop.Modules.Home.MenuItem;
using PawnShop.Modules.Home.ViewModels;
using PawnShop.Modules.Home.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using static PawnShop.Modules.Home.Views.Home;

namespace PawnShop.Modules.Home
{
    public class HomeModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public HomeModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.MenuRegion, typeof(HomeHamburgerMenuItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Home.Views.Home, HomeViewModel>();
        }
    }
}