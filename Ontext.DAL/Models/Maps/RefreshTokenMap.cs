using System.Data.Entity.ModelConfiguration;

namespace Ontext.DAL.Models.Maps
{
    public class RefreshTokenMap : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenMap()
        {
            ToTable("RefreshTokens");
        }
    }
}