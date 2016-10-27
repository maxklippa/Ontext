using System.IO;
using System.Web.Hosting;
using AutoMapper;
using Ontext.BLL.Providers.Base;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Objects;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.DAL.UnitOfWork.Contracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Ontext.Server.Core.Settings;

namespace Ontext.BLL.Providers
{
    public class MessagesProvider : HostService<IMessagesProvider>, IMessagesProvider
    {
        #region [ Private Fields ]

        #endregion // [ Private Fields ]

        #region [ Constructors ]

        public MessagesProvider(IServicesHost servicesHost, IUnitOfWork unitOfWork)
            : base(servicesHost, unitOfWork)
        {
        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

        public List<ApiMessage> GetUserMessages(Guid userId, bool unreadOnly = true)
        {
            var store = UnitOfWork.GetRepository<Message>().GetAll().Include(m => m.Contact)
                .Join(
                UnitOfWork.GetRepository<Contact>().GetAll().Include(c => c.Phone).Where(c => c.Phone.User != null && c.Phone.User.Id == userId),
                message => message.Contact.Id, 
                contact => contact.Id,
                (message, contact) => message).Where(message => !unreadOnly || !message.Read);

            return Mapper.Map<IQueryable<Message>, List<ApiMessage>>(store);
        }

        public void DeleteAll(Guid userId)
        {
            var messages = GetUserMessages(userId);
            messages.AddRange(GetUserMessages(userId, false));

            foreach (var message in messages)
            {
                Delete(message.Id);
            }
        }

        public int UnreadUserMessagesCount(Guid userId)
        {
            return UnitOfWork.GetRepository<Message>().GetAll().Include(m => m.Contact)
                .Join(
                    UnitOfWork.GetRepository<Contact>().GetAll().Include(c => c.Phone).Where(c => c.Phone.User != null && c.Phone.User.Id == userId),
                    message => message.Contact.Id,
                    contact => contact.Id,
                    (message, contact) => message).Count(message => !message.Read); 
        }

        public Guid Save(ApiMessage model)
        {
            var messageEntity = UnitOfWork.GetRepository<Message>().GetById(model.Id);
            
            if (messageEntity == null)
            {
                messageEntity = Mapper.Map<ApiMessage, Message>(model);

                SetMessageSenderName(model, messageEntity);

                UnitOfWork.GetRepository<Message>().Insert(messageEntity);
            }
            else
            {
                messageEntity = Mapper.Map(model, messageEntity);

                SetMessageSenderName(model, messageEntity);

                UnitOfWork.GetRepository<Message>().Update(messageEntity);
            }

            UnitOfWork.SaveChanges();

            return messageEntity.Id;
        }

        public List<ApiMessage> GetAll()
        {
            var store = UnitOfWork.GetRepository<Message>().GetAll().Include(m => m.Contact);

            return Mapper.Map<IQueryable<Message>, List<ApiMessage>>(store);
        }

        public ApiMessage GetById(Guid id)
        {
            var store = UnitOfWork.GetRepository<Message>().GetById(id);

            return (store != null) ? Mapper.Map<Message, ApiMessage>(store) : null;
        }

        public void Delete(ApiMessage model)
        {
            DeleteMessageImage(model.Id);

            UnitOfWork.GetRepository<Message>().Delete(model.Id);
            UnitOfWork.SaveChanges();
        }

        public void Delete(Guid id)
        {
            DeleteMessageImage(id);

            UnitOfWork.GetRepository<Message>().Delete(id);
            UnitOfWork.SaveChanges();
        }

        private void DeleteMessageImage(Guid id)
        {
            var deletedMessage = UnitOfWork.GetRepository<Message>().GetById(id);

            if (deletedMessage == null || string.IsNullOrWhiteSpace(deletedMessage.Image)) return;

            var filePath = HostingEnvironment.MapPath(string.Format("{0}{1}{2}", OntextSettings.UploadImageDirectoryPath, deletedMessage.Image, OntextSettings.UploadImageExtension));

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        #endregion // [ Public Methods ]

        #region [ Private Methods ]

        private void SetMessageSenderName(ApiMessage message, Message messageEntity)
        {
            var user = UnitOfWork.GetRepository<OntextUser>().GetAll().Include(c => c.Phones).First(u => u.Id == message.UserId);

            if (user == null)
            {
                throw new ArgumentException("Related entities do not exist.");
            }

            var receiverPhone =
                UnitOfWork.GetRepository<Phone>()
                    .GetAll()
                    .Include(p => p.Contacts)
                    .First(p => p.Contacts.Any(c => c.Id == message.ContactId));

            var receiverUser = !receiverPhone.UserId.HasValue
                ? null
                : UnitOfWork.GetRepository<OntextUser>().GetAll().FirstOrDefault(u => u.Id == receiverPhone.UserId);

            if (receiverUser != null)
            {
                var senderContact = receiverUser.Contacts.FirstOrDefault(c => user.Phones.Contains(c.Phone));
                messageEntity.SenderName = senderContact != null ? senderContact.Name : user.Phones.First().Number;
            }
            else
            {
                messageEntity.SenderName = user.Phones.First().Number;
            }
        }

        #endregion // [ Private Methods ]
    }
}