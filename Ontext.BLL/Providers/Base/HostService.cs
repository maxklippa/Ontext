using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.BLL.Providers.Base
{
    /// <summary>
    /// Base services abstract class
    /// </summary>
    public abstract class HostService<T> : IService where T : IService
    {
        /// <summary>
        /// The unit of work.
        /// </summary>
        protected readonly IUnitOfWork UnitOfWork;
        /// <summary>
        /// Creates 
        /// </summary>
        /// <param name="servicesHost"></param>
        /// <param name="unitOfWork"></param>
        protected HostService(IServicesHost servicesHost, IUnitOfWork unitOfWork)
        {
            this.UnitOfWork = unitOfWork;
            servicesHost.Register((T)(this as IService));
        }
    }
}