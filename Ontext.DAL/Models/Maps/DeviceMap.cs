using System.Data.Entity.ModelConfiguration;

namespace Ontext.DAL.Models.Maps
{
    public class DeviceMap : EntityTypeConfiguration<Device>
    {
        public DeviceMap()
        {
            ToTable("Devices");

            HasKey(d => d.Id);

            HasRequired(d => d.User).WithMany(u => u.Devices);
        }
    }
}