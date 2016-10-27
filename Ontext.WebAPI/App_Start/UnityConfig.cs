using System;
using System.Linq;
using System.Reflection;
using Microsoft.Owin.Security.MicrosoftAccount;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ObjectBuilder2;
using Ontext.BLL.ServicesHost;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.DAL.UnitOfWork;
using Unity.AutoRegistration;

using Ontext.BLL.Providers.Contracts;
using Ontext.DAL.Context;
using Ontext.DAL.Context.Contracts;
using Ontext.DAL.Repositories.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.WebAPI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> Container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return Container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // register Db context
            container.RegisterType<IOntextDbContext, OntextDbContext>(new HierarchicalLifetimeManager());

            // register all repositories
            container
               .ConfigureAutoRegistration()
               .LoadAssemblyFrom(Assembly.GetExecutingAssembly().Location)
               .ExcludeAssemblies(a => !a.FullName.ToLowerInvariant().StartsWith("ontext.dal"))
               .Include((type) => type.Implements<ICustomRepository>() && type.IsClass && !type.IsAbstract && !type.IsGenericType, Then.Register().UsingLifetime<HierarchicalLifetimeManager>().As<ICustomRepository>().WithName(t => t.FullName))
               .ApplyAutoRegistration();

            // register all services
            container
               .ConfigureAutoRegistration()
               .LoadAssemblyFrom(Assembly.GetExecutingAssembly().Location)
               .ExcludeAssemblies(a => !a.FullName.ToLowerInvariant().StartsWith("ontext.bll"))
               .Include((type) => type.Implements<IService>() && type.IsClass && !type.IsAbstract && !type.IsGenericType, Then.Register().UsingLifetime<HierarchicalLifetimeManager>().As<IService>().WithName(t => t.FullName))
               .ApplyAutoRegistration();

            // register Unit of Work
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager(), new InjectionFactory(c =>
            {
                var uow = new UnitOfWork(c.Resolve<IOntextDbContext>());

                c.Registrations
                .Where(item => item.RegisteredType == typeof(ICustomRepository) && !item.MappedToType.IsInterface && !item.MappedToType.IsGenericType && !item.MappedToType.IsAbstract && !String.IsNullOrEmpty(item.Name))
                .ForEach(item => c.Resolve<ICustomRepository>(item.Name, new ResolverOverride[] {
                    new ParameterOverride("unitOfWork", uow)
                }));

                return uow;
            }));

            // register services host
            container.RegisterType<IServicesHost, ServicesHost>(new HierarchicalLifetimeManager(), new InjectionFactory(c =>
            {
                var host = new ServicesHost();
                var uow = c.Resolve<IUnitOfWork>();

                c.Registrations
                .Where(item => item.RegisteredType == typeof(IService) && !item.MappedToType.IsInterface && !item.MappedToType.IsGenericType && !item.MappedToType.IsAbstract && !String.IsNullOrEmpty(item.Name))
                .ForEach(item => c.Resolve<IService>(item.Name, new ResolverOverride[] {
                    new ParameterOverride("servicesHost", host),
                    new ParameterOverride("unitOfWork", uow)
                }));

                return host;
            }));
        }
    }
}
