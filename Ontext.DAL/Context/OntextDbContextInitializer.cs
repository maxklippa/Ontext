using System.Data.Entity;

namespace Ontext.DAL.Context
{
    public class MercuryDbContextInitializer : MigrateDatabaseToLatestVersion<OntextDbContext, OntextDbContextConfiguration>
    {
    }
}