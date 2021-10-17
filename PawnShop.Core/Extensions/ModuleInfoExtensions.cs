using PawnShop.Core.Attributes;
using PawnShop.Core.Events;
using PawnShop.Core.Models.ModuleVisibility;
using PawnShop.Core.SharedVariables;
using Prism.Events;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PawnShop.Core.Extensions
{
    public static class ModuleInfoExtensions
    {
        public static bool HasCurrentUserPrivilege(this IModuleInfo moduleInfo, ISessionContext sessionContext)
        {
            var modulePrivilege = GetModulePrivilege(moduleInfo);
            var hasPrivilege = CheckUserPrivilege(modulePrivilege, sessionContext);
            return hasPrivilege;
        }

        public static void ShowModule(this IModuleInfo moduleInfo, IModuleManager moduleManager, IEventAggregator ea)
        {
            if (moduleInfo.State == ModuleState.Initialized)
            {
                ea.GetEvent<ModuleVisibilityEvent>().Publish(new ModuleVisibility() { IsVisible = true, ModuleName = moduleInfo.ModuleName });
            }
            else if (moduleInfo.State == ModuleState.NotStarted)
            {
                moduleManager.LoadModule(moduleInfo.ModuleName);
            }
        }

        public static void HideModule(this IModuleInfo moduleInfo, IEventAggregator ea)
        {
            if (moduleInfo.State == ModuleState.Initialized)
            {
                ea.GetEvent<ModuleVisibilityEvent>().Publish(new ModuleVisibility() { IsVisible = false, ModuleName = moduleInfo.ModuleName });
            }
        }

        public static IEnumerable<T> OrderModules<T>(this IEnumerable<T> moduleInfos) where T : IModuleInfo
        {
            return moduleInfos.OrderBy(m => GetModuleOrder(m));
        }

        private static string GetModulePrivilege(IModuleInfo moduleInfo)
        {
            var type = Type.GetType(moduleInfo.ModuleType);
            var privilegeAttribute = GetCustomAttribute<PrivilegeAttribute>(type);

            if (privilegeAttribute is null)
                throw new NullReferenceException("Module doesn't have PrivilegeAttribute.");

            return privilegeAttribute.Privilege;
        }

        private static int GetModuleOrder(IModuleInfo moduleInfo)
        {
            var type = Type.GetType(moduleInfo.ModuleType);
            var orderAttribute = GetCustomAttribute<OrderAttribute>(type);

            if (orderAttribute is null)
                throw new NullReferenceException("Module doesn't have OrderAttribute.");

            return orderAttribute.Order;
        }

        private static T GetCustomAttribute<T>(Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>().FirstOrDefault();
        }

        private static bool CheckUserPrivilege(string modulePrivilege, ISessionContext sessionContext)
        {
            return modulePrivilege switch
            {
                "PawnShopTabs" => sessionContext.LoggedPerson.Privilege.PawnShopTabs,
                "SettingsTab" => sessionContext.LoggedPerson.Privilege.SettingsTab,
                "WorkersTab" => sessionContext.LoggedPerson.Privilege.WorkersTab,
                "Login" => true,
                _ => throw new Exception("Module has an unknown PrivilegeAttribute."),
            };
        }
    }
}
