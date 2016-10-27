using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Objects;
using Ontext.Core.Responses;
using Ontext.Core.Responses.Base;

namespace Ontext.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Contacts")]
    public class ContactsController : BaseApiController
    {
        #region [ Constructors ]

        public ContactsController(IServicesHost servicesHost)
            : base(servicesHost)
        {
        }

        #endregion // [ Constructors ]

        #region [ Actions ]

        /// <summary>
        /// Get contacts for a given account
        /// </summary>
        /// <param name="id">Account ID</param>
        /// <returns></returns>
        [Route("ByAccount/Active/{id}")]
        [HttpGet]
        public JsonResult<ApiContactResponse> GetActiveByAccount(Guid id)
        {
            var result = new ApiContactResponse();

            try
            {
                result.Items = ServicesHost.GetService<IContactsProvider>().GetActiveContacts(id).ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("ByAccount/Blocked/{id}")]
        [HttpGet]
        public JsonResult<ApiContactResponse> GetBlockedByAccount(Guid id)
        {
            var result = new ApiContactResponse();

            try
            {
                result.Items = ServicesHost.GetService<IContactsProvider>().GetBlockedContacts(id).ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("ByAccount/{id}")]
        [HttpGet]
        public JsonResult<ApiContactResponse> GetByAccount(Guid id)
        {
            var result = new ApiContactResponse();

            try
            {
                result.Items = ServicesHost.GetService<IContactsProvider>().GetContacts(id).ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("GetLastModifiedContacts")]
        [HttpGet]
        public JsonResult<ApiContactResponse> GetLastModifiedContacts(Guid userId, long lastModifiedDateTicks)
        {
            var result = new ApiContactResponse();

            try
            {
                var lastModifiedDate = new DateTime(lastModifiedDateTicks);
                result.Items = ServicesHost.GetService<IContactsProvider>().GetLastModifiedContacts(userId, lastModifiedDate).ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("ByPhones/WithApp")]
        public JsonResult<ApiPhoneResponse> GetByPhonesWithApp([FromUri] string[] phones)
        {
            var result = new ApiPhoneResponse();

            try
            {
                result.Items = ServicesHost.GetService<IContactsProvider>().GetContactsWithApp(phones).ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Get a specified contact
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns></returns>
        public JsonResult<ApiContactResponse> Get(Guid id)
        {
            var result = new ApiContactResponse();

            try
            {
                var contact = ServicesHost.GetService<IContactsProvider>().GetById(id);
                result.Items = new[] { contact }.ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Create a new contact
        /// </summary>
        /// <param name="value">Contact to create</param>
        /// <returns></returns>
        public JsonResult<ApiBaseResponse> Post([FromBody]ApiContact value)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<IContactsProvider>().Save(value);
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        /// <summary>
        /// Update a contact
        /// </summary>
        /// <param name="value">Contact to update</param>
        /// <returns></returns>
        public JsonResult<ApiBaseResponse> Put([FromBody]ApiContact value)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<IContactsProvider>().Save(value);
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }
        
        /// <summary>
        /// Delete a contact
        /// </summary>
        /// <param name="id">Contact ID</param>
        /// <returns></returns>
        public JsonResult<ApiBaseResponse> Delete(Guid id)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<IContactsProvider>().Delete(id);
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("GetByPhoneNumber")]
        [HttpGet]
        public JsonResult<ApiContactResponse> GetByPhoneNumber(Guid userId, string phoneNumber)
        {
            var result = new ApiContactResponse();

            try
            {
                result.Items = new[] { ServicesHost.GetService<IContactsProvider>().GetByPhoneNumber(userId, phoneNumber) }.ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        } 

        #endregion // [ Actions ]
    }
}