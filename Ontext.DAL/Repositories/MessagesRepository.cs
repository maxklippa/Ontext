using System;
using System.Collections.Generic;
using System.Linq;
using Ontext.DAL.Context;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Base;
using Ontext.DAL.Repositories.Contracts;

namespace Ontext.DAL.Repositories
{
    public class MessagesRepository : BaseRepository, IMessagesRepository
    {
        #region [ Constructors ]

        public MessagesRepository(OntextDbContext databaseContext)
            :base(databaseContext)
        {

        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public Message GetById(Guid id)
        {
            return Context.Messages.FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Message> GetMessagesByUserId(Guid userId)
        {
            return Context.Messages.Where(m => m.Contact.Phone.User != null && m.Contact.Phone.User.Id == userId);
        }

        public bool Add(Message message)
        {
            Context.Messages.Add(message);

            return Context.SaveChanges() > 0;
        }

        public bool Update(Message message)
        {
            var oldMessage = GetById(message.Id);

            if (oldMessage == null)
                return false;

            oldMessage.Contact = message.Contact;
            oldMessage.CreationDate = message.CreationDate;
            oldMessage.Image = message.Image;
            oldMessage.Latitude = message.Latitude;
            oldMessage.Longitude = message.Longitude;
            oldMessage.Read = message.Read;
            oldMessage.SenderName = message.SenderName;
            oldMessage.SenderPhone = message.SenderPhone;
            oldMessage.Text = message.Text;

            return Context.SaveChanges() > 0;
        }

        public bool Delete(Guid id)
        {
            var message = GetById(id);

            if (message == null)
                return false;

            Context.Messages.Remove(message);

            return Context.SaveChanges() > 0;
        }

        #endregion // [ Public Methods ]
    }
}