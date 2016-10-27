using System.Data.Entity.ModelConfiguration;

namespace Ontext.DAL.Models.Maps
{
    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap()
        {
            ToTable("Messages");

            HasKey(m => m.Id);

            HasRequired(m => m.Contact).WithMany(c => c.Messages).WillCascadeOnDelete(true);
            HasRequired(m => m.Context).WithMany(c => c.Messages);
            HasRequired(m => m.User).WithMany(c => c.Messages);
        }
    }
}