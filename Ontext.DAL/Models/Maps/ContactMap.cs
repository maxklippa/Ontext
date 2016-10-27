using System.Data.Entity.ModelConfiguration;
using Newtonsoft.Json;

namespace Ontext.DAL.Models.Maps
{
    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            ToTable("Contacts");

            HasKey(c => c.Id);

            HasRequired(c => c.Context).WithMany(ctx => ctx.Contacts);
            HasRequired(c => c.User).WithMany(u => u.Contacts);
            HasRequired(c => c.Phone).WithMany(p => p.Contacts);

            HasMany(c => c.Messages).WithRequired(m => m.Contact);
        }
    }
}