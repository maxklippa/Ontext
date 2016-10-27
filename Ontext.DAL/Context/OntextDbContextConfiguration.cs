using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.AspNet.Identity;
using Ontext.Core.Enums;
using Ontext.DAL.Models;
using Ontext.Server.Core.Settings;
using Ontext.Server.Core.Utils;

namespace Ontext.DAL.Context
{
    public sealed class OntextDbContextConfiguration : DbMigrationsConfiguration<OntextDbContext>
    {

        /// <summary>
        /// Cretes configutation instance
        /// </summary>
        public OntextDbContextConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        /// <summary>
        /// On db context seed
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(OntextDbContext context)
        {
            if (context.Set<Client>().Any())
            {
                return;
            }
            context.Set<Client>().AddRange(BuildClientsList());
            context.SaveChanges();
        }

        private static IEnumerable<Client> BuildClientsList()
        {

            var clientsList = new List<Client> 
            {
                new Client
                { 
                    Id = OntextSettings.MobileClientId, 
                    Secret = HashHelper.GetHash(OntextSettings.MobileClientSecret), 
                    Name = OntextSettings.MobileClientName, 
                    ApplicationType = OntextSettings.MobileClientApplicationType, 
                    Active = OntextSettings.MobileClientActive, 
                    RefreshTokenLifeTime = OntextSettings.MobileClientRefreshTokenLifeTime, 
                    AllowedOrigin = OntextSettings.MobileClientAllowedOrigin
                }
            };

            return clientsList;
        }
    }
}