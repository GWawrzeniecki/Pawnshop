using PawnShop.Core.Privileges;
using PawnShop.Core.SharedVariables;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Globalization;
using System.Linq;

namespace PawnShop.Core.Modules
{
    public class PrivilegeBasedModuleInitializer : IModuleInitializer
    {
        private readonly IContainerExtension _containerExtension;

        /// <summary>
        /// Initializes a new instance of <see cref="ModuleInitializer"/>.
        /// </summary>
        /// <param name="containerExtension">The container that will be used to resolve the modules by specifying its type.</param>
        public PrivilegeBasedModuleInitializer(IContainerExtension containerExtension)
        {
            this._containerExtension = containerExtension ?? throw new ArgumentNullException(nameof(containerExtension));
        }

        /// <summary>
        /// Initializes the specified module.
        /// </summary>
        /// <param name="moduleInfo">The module to initialize</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Catches Exception to handle any exception thrown during the initialization process with the HandleModuleInitializationError method.")]
        public void Initialize(IModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            IModule moduleInstance = null;
            try
            {
                if (UserHasPrivileges(moduleInfo))
                {
                    moduleInstance = this.CreateModule(moduleInfo);
                    if (moduleInstance != null)
                    {
                        moduleInstance.RegisterTypes(_containerExtension);
                        moduleInstance.OnInitialized(_containerExtension);
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleModuleInitializationError(
                    moduleInfo,
                    moduleInstance?.GetType().Assembly.FullName,
                    ex);
            }
        }

        private bool UserHasPrivileges(IModuleInfo moduleInfo)
        {
            var modulePrivilege = GetModulePrivilege(moduleInfo);
            var hasPrivilege = CheckUserPrivilege(modulePrivilege);
            return hasPrivilege;
        }

        private bool CheckUserPrivilege(string modulePrivilege)
        {
            var sessionContext = _containerExtension.Resolve<ISessionContext>();

            return modulePrivilege switch
            {
                "PawnShopTabs" => sessionContext.LoggedPerson.Privilege.PawnShopTabs,
                "SettingsTab" => sessionContext.LoggedPerson.Privilege.SettingsTab,
                "WorkersTab" => sessionContext.LoggedPerson.Privilege.WorkersTab,
                "Login" => true,
                _ => throw new Exception("Module has an unknown PrivilegeAttribute."),
            };
        }

        private string GetModulePrivilege(IModuleInfo moduleInfo)
        {
            var type = Type.GetType(moduleInfo.ModuleType);
            var privilegeAttribute = GetCustomAttribute<PrivilegeAttribute>(type);

            if (privilegeAttribute is null)
                throw new NullReferenceException("Module doesn't have PrivilegeAttribute.");

            return privilegeAttribute.Privilege;
        }

        private T GetCustomAttribute<T>(Type type)
        {
            return type.GetCustomAttributes(typeof(T), true).OfType<T>().FirstOrDefault();
        }

        /// <summary>
        /// Handles any exception occurred in the module Initialization process,
        /// This method can be overridden to provide a different behavior.
        /// </summary>
        /// <param name="moduleInfo">The module metadata where the error happened.</param>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="exception">The exception thrown that is the cause of the current error.</param>
        /// <exception cref="ModuleInitializeException"></exception>
        public virtual void HandleModuleInitializationError(IModuleInfo moduleInfo, string assemblyName, Exception exception)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            Exception moduleException;

            if (exception is ModuleInitializeException)
            {
                moduleException = exception;
            }
            else
            {
                if (!string.IsNullOrEmpty(assemblyName))
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, assemblyName, exception.Message, exception);
                }
                else
                {
                    moduleException = new ModuleInitializeException(moduleInfo.ModuleName, exception.Message, exception);
                }
            }

            throw moduleException;
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="moduleInfo">The module to create.</param>
        /// <returns>A new instance of the module specified by <paramref name="moduleInfo"/>.</returns>
        protected virtual IModule CreateModule(IModuleInfo moduleInfo)
        {
            if (moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            return this.CreateModule(moduleInfo.ModuleType);
        }

        /// <summary>
        /// Uses the container to resolve a new <see cref="IModule"/> by specifying its <see cref="Type"/>.
        /// </summary>
        /// <param name="typeName">The type name to resolve. This type must implement <see cref="IModule"/>.</param>
        /// <returns>A new instance of <paramref name="typeName"/>.</returns>
        protected virtual IModule CreateModule(string typeName)
        {
            Type moduleType = Type.GetType(typeName);
            if (moduleType == null)
            {
                throw new ModuleInitializeException(string.Format(CultureInfo.CurrentCulture, "FailedToGetType", typeName));
            }

            return (IModule)_containerExtension.Resolve(moduleType);
        }
    }
}