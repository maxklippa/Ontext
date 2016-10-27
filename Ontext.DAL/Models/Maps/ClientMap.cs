using System.Data.Entity.ModelConfiguration;

namespace Ontext.DAL.Models.Maps
{
    public class ClientMap : EntityTypeConfiguration<Client>
    {
        public ClientMap()
        {
            ToTable("Clients");
        }
    }
}