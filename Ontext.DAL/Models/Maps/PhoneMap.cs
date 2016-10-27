using System.Data.Entity.ModelConfiguration;

namespace Ontext.DAL.Models.Maps
{
    public class PhoneMap : EntityTypeConfiguration<Phone>
    {
        public PhoneMap()
        {
            ToTable("Phones");

            HasKey(p => p.Id);

            HasMany(p => p.Contacts).WithRequired(c => c.Phone);

            HasOptional(p => p.User).WithMany(u => u.Phones);
        }
    }
}