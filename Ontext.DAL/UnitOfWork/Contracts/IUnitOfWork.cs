using System;
using Ontext.DAL.Context.Contracts;
using Ontext.DAL.Repositories.Contracts;

namespace Ontext.DAL.UnitOfWork.Contracts
{

    /// <summary>
    /// UOW contract
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Register custom repository
        /// </summary>
        /// <param name="customRepository">Repository instance</param>
        void RegisterCustomRepository<T>(T customRepository) where T : ICustomRepository;
        /// <summary>
        /// Gets application context instance
        /// </summary>
        IOntextDbContext Context { get; }
        /// <summary>
        /// Get repository by entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        IEntityRepository<T> GetRepository<T>();
        /// <summary>
        /// Get repository by repository type
        /// </summary>
        /// <typeparam name="T">Custom repository type</typeparam>
        /// <returns>Custom repository instance</returns>
        T GetCustomRepository<T>() where T : ICustomRepository;
        /// <summary>
        /// Rollback uncommited changes
        /// </summary>
        void RollBack();
        /// <summary>
        /// Commit changes
        /// </summary>
        void SaveChanges();
    }
}
