using System.Data.Entity.ModelConfiguration;
using Ontext.DAL.Identity;

namespace Ontext.DAL.Models.Maps
{
    public class OntextUserMap : EntityTypeConfiguration<OntextUser>
    {
        public OntextUserMap()
        {
            HasKey(u => u.Id);

            HasMany(u => u.Contacts).WithRequired(c => c.User);
            HasMany(u => u.Phones).WithOptional(p => p.User);
            HasMany(u => u.Devices).WithRequired(d => d.User);
            HasMany(u => u.Messages).WithRequired(m => m.User);

            HasOptional(u => u.Settings).WithRequired(s => s.User);
        }
    }
}