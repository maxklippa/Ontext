using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Ontext.DAL.Context.Contracts;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.DAL.Models.Maps;

namespace Ontext.DAL.Context
{
    /// <summary>
    /// Main application db context implementation
    /// </summary>
    public class OntextDbContext : IdentityDbContext<OntextUser, OntextRole, Guid, OntextUserLogin, OntextUserRole, OntextUserClaim>, IOntextDbContext
    {
        #region [ Constructors ]

        /// <summary>
        /// Creates context instance
        /// </summary>
        public OntextDbContext()
            : base("OntextConnection")
        {
            Configuration.LazyLoadingEnabled = false;
        }

        #endregion // [ Constructors ]

        /// <summary>
        /// Gest base db context instance
        /// </summary>
        public DbContext DbContext
        {
            get
            {
                return this;
            }
        }

        #region [ Overridden Methods ]

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Identity
            modelBuilder.Entity<OntextUser>().ToTable("Users");
            modelBuilder.Entity<OntextRole>().ToTable("Roles");
            modelBuilder.Entity<OntextUserRole>().ToTable("UserRoles");
            modelBuilder.Entity<OntextUserLogin>().ToTable("UserLogins");
            modelBuilder.Entity<OntextUserClaim>().ToTable("UserClaims");

            #region Mapping

            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new ContextMap());
            modelBuilder.Configurations.Add(new MessageMap());
            modelBuilder.Configurations.Add(new PhoneMap());
            modelBuilder.Configurations.Add(new ClientMap());
            modelBuilder.Configurations.Add(new RefreshTokenMap());
            modelBuilder.Configurations.Add(new SettingsMap());

            #endregion

            #region Conventions

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            #endregion // Conventions
        }

        #endregion // [ Overridden Methods ]
    }
}
