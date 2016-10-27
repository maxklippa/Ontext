using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Results;
using Ontext.BLL.Identity.Services;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.Services.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Enums;
using Ontext.Core.Objects;
using Ontext.Core.Responses;
using Ontext.Core.Responses.Base;
using Ontext.Server.Core.Settings;
using PushSharp;
using PushSharp.Apple;
using PushSharp.Core;
using Twilio;

namespace Ontext.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Messages")]
    public class MessagesController : BaseApiController
    {
        #region [ Constructors ]

        public MessagesController(IServicesHost servicesHost)
            : base(servicesHost)
        {
        }

        #endregion // [ Constructors ]

        #region [ Actions ]

        /// <summary>
        /// Get messages for a given account
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ByAccount/{id}")]
        public JsonResult<ApiMessageResponse> GetByAccount(Guid id)
        {
            var result = new ApiMessageResponse();

            try
            {
                result.Items = ServicesHost.GetService<IMessagesProvider>().GetUserMessages(id, false).ToList();
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Get unread messages for a given account
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns></returns>
        [HttpGet]
        [Route("ByAccount/{id}/Unread")]
        public JsonResult<ApiMessageResponse> GetUnreadByAccount(Guid id)
        {
            var result = new ApiMessageResponse();

            try
            {
                result.Items = ServicesHost.GetService<IMessagesProvider>().GetUserMessages(id).ToList();
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [HttpPost]
        [Route("SendMessage")]
        public JsonResult<ApiBaseResponse> SendMessage([FromBody] ApiMessage message)
        {
            var result = new ApiBaseResponse();

            try
            {
                var messageId = ServicesHost.GetService<IMessagesProvider>().Save(message);
                message = ServicesHost.GetService<IMessagesProvider>().GetById(messageId);

                var contact = ServicesHost.GetService<IContactsProvider>().GetById(message.ContactId);
                var receiverUser = UserManager.FindByPhoneNumber(contact.PhoneNumber);

                if (receiverUser == null)
                {
                    var smsMessageText = message.GetSmsMessageText(message);
                    ServicesHost.GetService<ISmsService>().SendSmsMessage(contact.PhoneNumber, smsMessageText);
                }
                else
                {
                    var userDeviceTokens = ServicesHost.GetService<IDevicesProvider>().GetUserDevices(receiverUser.Id).Select(d => d.Token);
                    var messagesCount = ServicesHost.GetService<IMessagesProvider>().UnreadUserMessagesCount(receiverUser.Id);
                    var messageText = message.GetPushNotificationText();

                    Task.Run(() => ServicesHost.GetService<IApplePushNotificationService>().SendPushNotifications(userDeviceTokens, messageText, messagesCount, messageId.ToString()));
                }
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Get a specified message
        /// </summary>
        /// <param name="id">Message ID</param>
        /// <returns></returns>
        public JsonResult<ApiMessageResponse> Get(Guid id)
        {
            var result = new ApiMessageResponse();

            try
            {
                var message = ServicesHost.GetService<IMessagesProvider>().GetById(id);
                result.Items = new[] { message }.ToList();
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Create a new message
        /// </summary>
        /// <param name="value">Message to create</param>
        /// <returns></returns>
        public JsonResult<ApiBaseResponse> Post([FromBody]ApiMessage value)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<IMessagesProvider>().Save(value);
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Update a message
        /// </summary>
        /// <param name="value">Message to update</param>
        /// <returns></returns>
        public JsonResult<ApiBaseResponse> Put([FromBody]ApiMessage value)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<IMessagesProvider>().Save(value);
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Delete a message
        /// </summary>
        /// <param name="id">Message ID</param>
        /// <returns></returns>
        public JsonResult<ApiBaseResponse> Delete(Guid id)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<IMessagesProvider>().Delete(id);
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [HttpDelete]
        [Route("DeleteAll/{userId}")]
        public JsonResult<ApiBaseResponse> DeleteAll(Guid userId)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<IMessagesProvider>().DeleteAll(userId);
            }
            catch (Exception ex)
            {
                result.Status = ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        #endregion // [ Actions ]
    }
}
