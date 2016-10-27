using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading;
using Ontext.DAL.Context.Contracts;
using Ontext.DAL.Repositories.Base;
using Ontext.DAL.Repositories.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;

namespace Ontext.DAL.UnitOfWork
{

    /// <summary>
    /// UOW interface implementation
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {

        /// <summary>
        /// Holds registered repositories
        /// </summary>
        private readonly Dictionary<Type, IEntityRepository> enityRepositories;

        /// <summary>
        /// Holds registered custom repositories
        /// </summary>
        private readonly Dictionary<Type, ICustomRepository> customRepositories;

        /// <summary>
        /// Thread safe locker
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// Gets application db context instance
        /// </summary>
        public IOntextDbContext Context { get; private set; }

        /// <summary>
        /// Create UOW instance
        /// </summary>
        /// <param name="context">Application db context</param>
        public UnitOfWork(IOntextDbContext context)
        {
            enityRepositories = new Dictionary<Type, IEntityRepository>();
            customRepositories = new Dictionary<Type, ICustomRepository>();
            Context = context;
        }

        /// <summary>
        /// Register custom repository
        /// </summary>
        /// <param name="customRepository">Custom repository instance</param>
        public void RegisterCustomRepository<T>(T customRepository) where T : ICustomRepository
        {
            if (!customRepositories.ContainsKey(typeof(T)))
                customRepositories.Add(typeof(T), customRepository);
        }

        /// <summary>
        /// Get repository by entity type
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>Repository instance</returns>
        public IEntityRepository<T> GetRepository<T>()
        {
            // check if repository exist in cache
            if (enityRepositories.ContainsKey(typeof(T)))
                return enityRepositories[typeof(T)] as IEntityRepository<T>;
            // if not then create a new instance and add to cache
            var repositoryType = typeof(EntityRepository<>).MakeGenericType(typeof(T));
            var repository = (IEntityRepository<T>)Activator.CreateInstance(repositoryType, this.Context);
            enityRepositories.Add(typeof(T), repository);

            return repository;
        }

        /// <summary>
        /// Get repository by repository type
        /// </summary>
        /// <typeparam name="T">Custom repository type</typeparam>
        /// <returns>Custom repository instance</returns>
        public T GetCustomRepository<T>() where T : ICustomRepository
        {
            // check if repository exist in cache
            if (customRepositories.ContainsKey(typeof(T)))
                return (T)customRepositories[typeof(T)];

            return default(T);
        }

        /// <summary>
        /// The roll back.
        /// </summary>
        public void RollBack()
        {
            var changedEntries = this.Context.DbContext.ChangeTracker.Entries().Where(x => x.State != EntityState.Unchanged).ToList();

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Modified))
            {
                entry.CurrentValues.SetValues(entry.OriginalValues);
                entry.State = EntityState.Unchanged;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Added))
            {
                entry.State = EntityState.Detached;
            }

            foreach (var entry in changedEntries.Where(x => x.State == EntityState.Deleted))
            {
                entry.State = EntityState.Unchanged;
            }
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void SaveChanges()
        {

            try
            {
                Monitor.Enter(lockObject);
                this.Context.DbContext.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        public void Dispose()
        {
        }
    }
}
