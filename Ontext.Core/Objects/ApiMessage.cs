using System;
using Ontext.Core.Enums;
using Ontext.Core.Objects.Base;

namespace Ontext.Core.Objects
{
    public class ApiMessage : ApiEntity
    {
        public string Text { get; set; }
        public string Image { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public Guid UserId { get; set; }
        public bool Read { get; set; }
        public DateTime CreationDate { get; set; }
        public MessageType Type { get; set; }
        public string SenderName { get; set; }
        public Guid ContactId { get; set; }
        public Guid ContextId { get; set; }

        public string GetPushNotificationText()
        {
            var messageText = string.Empty;
            switch (Type)
            {
                case MessageType.Text:
                    messageText = string.Format("{0} says: \"{1}\"", SenderName, Text);
                    break;
                case MessageType.Image:
                    messageText = string.Format("{0} sent you an image.", SenderName);
                    break;
                case MessageType.Location:
                    messageText = string.Format("{0} is located here.", SenderName);
                    break;
            }
            return messageText;
        }

        public string GetSmsMessageText(ApiMessage message)
        {
            var smsMessageText = string.Empty;
            switch (message.Type)
            {
                case MessageType.Text:
                    smsMessageText = string.Format("{0} says: \"{1}\"", message.SenderName, message.Text);
                    break;
                case MessageType.Image:
                    smsMessageText = "image";
                    break;
                case MessageType.Location:
                    smsMessageText = "location";
                    break;
            }
            return smsMessageText;
        }
    }
}