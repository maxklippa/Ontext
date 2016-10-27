using System.Data.Entity.ModelConfiguration;

namespace Ontext.DAL.Models.Maps
{
    public class ContextMap : EntityTypeConfiguration<Context>
    {
        public ContextMap()
        {
            ToTable("Contexts");

            HasKey(c => c.Id);

            HasMany(ctx => ctx.Contacts).WithRequired(c => c.Context);
            HasMany(c => c.Messages).WithRequired(m => m.Context);
        }
    }
}