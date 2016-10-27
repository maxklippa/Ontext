using System.Data.Entity.ModelConfiguration;

namespace Ontext.DAL.Models.Maps
{
    public class SettingsMap : EntityTypeConfiguration<Settings>
    {
        public SettingsMap()
        {
            ToTable("Settings");

            HasKey(s => s.Id);

            HasRequired(s => s.User).WithOptional(u => u.Settings);
        }
    }
}