using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PawnShop.Core.ScopedRegion
{
    public interface IScopedWindow
    {
        void RegisterViews(IContainerProvider containerProvider, IRegionManager scopedRegionManager);
    }
}
