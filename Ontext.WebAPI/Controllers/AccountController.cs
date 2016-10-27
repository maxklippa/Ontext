using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Ontext.BLL.Providers.Contracts;
using Ontext.BLL.Services.Contracts;
using Ontext.BLL.ServicesHost.Contracts;
using Ontext.Core.Objects;
using Ontext.Core.Responses;
using Ontext.Core.Responses.Base;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.Server.Core.Settings;
using Ontext.Server.Core.Utils;

namespace Ontext.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        public AccountController(IServicesHost servicesHost)
            : base(servicesHost)
        {
        }

        [AllowAnonymous]
        [Route("LoginByPhone")]
        [HttpPost]
        public JsonResult<ApiBaseResponse> LoginByPhone([FromUri] string phoneNumber)
        {
            var result = new ApiBaseResponse();

            try
            {
                var user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindByPhoneNumber(phoneNumber));

                if (user == null)
                {
                    var userEntity = new OntextUser();

                    var phone = ServicesHost.GetService<IPhonesProvider>().GetByPhoneNumber(phoneNumber) ?? new ApiPhone
                    {
                        Number = phoneNumber
                    };

                    userEntity.UserName = Guid.NewGuid().ToString();
                    userEntity.TwoFactorEnabled = true;

                    var userPassword = OntextSettings.UserDefaultPassword;

                    var r = UserManager.Create(userEntity, userPassword);

                    if (!r.Succeeded)
                    {
                        result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                        return Json(result);
                    }

                    phone.UserId = userEntity.Id;
                    ServicesHost.GetService<IPhonesProvider>().Save(phone);

                    user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindByPhoneNumber(phoneNumber));
                }

                var purpose = IdentityUserTokenHelper.GenerateTokenPurpose(IdentityUserTokenHelper.TokenPurpose.Loging, phoneNumber);
                var token = UserManager.GenerateUserToken(purpose, user.Id);
#if DEBUG
                UserManager.SendEmail(ConfigurationManager.AppSettings["Email"], "Security Code", token);
#else
                UserManager.SendSms(phoneNumber, token);
#endif
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [AllowAnonymous]
        [Route("LoginByEmail")]
        [HttpPost]
        public JsonResult<ApiBaseResponse> LoginByEmail([FromUri] string email)
        {
            var result = new ApiBaseResponse();

            try
            {
                var user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindByEmail(email));

                if (user == null)
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                }
                else
                {
                    var purpose = IdentityUserTokenHelper.GenerateTokenPurpose(IdentityUserTokenHelper.TokenPurpose.Loging, email);
                    var token = UserManager.GenerateUserToken(purpose, user.Id); 
                    UserManager.SendEmail(email, "Security Code", token);
                }
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("SendCode/BySMS")]
        public JsonResult<ApiBaseResponse> SendCodeBySms(Guid userId, string phoneNumber)
        {
            var result = new ApiBaseResponse();

            try
            {
                var user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindById(userId));

                if (user == null)
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                }
                else
                {
                    var purpose = IdentityUserTokenHelper.GenerateTokenPurpose(IdentityUserTokenHelper.TokenPurpose.Editing, phoneNumber);
                    var token = UserManager.GenerateUserToken(purpose, user.Id);
                    ServicesHost.GetService<ISmsService>().SendSmsMessage(phoneNumber, token);
                }
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("SendCode/ByEmail")]
        public JsonResult<ApiBaseResponse> SendCodeByEmail(Guid userId, string email)
        {
            var result = new ApiBaseResponse();

            try
            {
                var user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindById(userId));

                if (user == null)
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                }
                else
                {
                    var purpose = IdentityUserTokenHelper.GenerateTokenPurpose(IdentityUserTokenHelper.TokenPurpose.Editing, email);
                    var token = UserManager.GenerateUserToken(purpose, user.Id); 
                    
                    UserManager.SendEmail(email, "Security Code", token);
                }
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("AddPhoneNumber")]
        public JsonResult<ApiBaseResponse> AddPhoneNumber([FromUri] Guid userId, string phoneNumber, string code)
        {
            var result = new ApiBaseResponse();

            try
            {
                var user = UserManager.FindById(userId);
                var phone = ServicesHost.GetService<IPhonesProvider>().GetByPhoneNumber(phoneNumber);

                var validCode = UserManager.VerifyUserToken(userId, IdentityUserTokenHelper.GenerateTokenPurpose(IdentityUserTokenHelper.TokenPurpose.Editing, phoneNumber), code);

                if (!validCode || user == null || (phone != null && phone.UserId.HasValue))
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                    return Json(result);
                }

                if (phone == null)
                {
                    phone = new ApiPhone {Number = phoneNumber, UserId = user.Id};
                    ServicesHost.GetService<IPhonesProvider>().Save(phone);
                }
                else
                {
                    phone.UserId = user.Id;
                    ServicesHost.GetService<IPhonesProvider>().Save(phone);
                }
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("ChangePhoneNumber")]
        public JsonResult<ApiBaseResponse> ChangePhoneNumber([FromUri] Guid userId, string oldPhoneNumber, string newPhoneNumber, string code)
        {
            var result = new ApiBaseResponse();

            try
            {
                var user = UserManager.FindById(userId);
                var oldPhone = ServicesHost.GetService<IPhonesProvider>().GetByPhoneNumber(oldPhoneNumber);
                var newPhone = ServicesHost.GetService<IPhonesProvider>().GetByPhoneNumber(newPhoneNumber);

                var validCode = UserManager.VerifyUserToken(userId, IdentityUserTokenHelper.GenerateTokenPurpose(IdentityUserTokenHelper.TokenPurpose.Editing, newPhoneNumber), code);

                if (!validCode || user == null || (oldPhone == null || oldPhone.UserId != userId) 
                    || (newPhone != null && newPhone.UserId.HasValue && newPhone.UserId != Guid.Empty))
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                    return Json(result);
                }

                if (newPhone == null)
                {
                    newPhone = new ApiPhone { Number = newPhoneNumber, UserId = user.Id };
                    ServicesHost.GetService<IPhonesProvider>().Save(newPhone);
                }
                else
                {
                    newPhone.UserId = user.Id;
                    ServicesHost.GetService<IPhonesProvider>().Save(newPhone);
                }

                oldPhone.UserId = null;
                ServicesHost.GetService<IPhonesProvider>().Save(oldPhone);
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("ChangeEmail")]
        public JsonResult<ApiBaseResponse> ChangeEmail([FromUri] Guid userId, string email, string code)
        {
            var result = new ApiBaseResponse();

            try
            {
                var user = UserManager.FindById(userId);

                var validCode = UserManager.VerifyUserToken(userId, IdentityUserTokenHelper.GenerateTokenPurpose(IdentityUserTokenHelper.TokenPurpose.Editing, email), code);

                if (!validCode || user == null)
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                    return Json(result);
                }

                user.Email = email;

                if (!UserManager.Update(user).Succeeded)
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                }
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("GetByPhone")]
        public JsonResult<ApiUserResponse> GetByPhone(string phoneNumber)
        {
            var result = new ApiUserResponse();

            try
            {
                var user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindByPhoneNumber(phoneNumber));
                result.Items = new[] { user }.ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("GetByEmail")]
        public JsonResult<ApiUserResponse> GetByEmail(string email)
        {
            var result = new ApiUserResponse();

            try
            {
                var user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindByEmail(email));
                result.Items = new[] { user }.ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("GetUserPhones")]
        public JsonResult<ApiPhoneResponse> GetUserPhones(Guid userId)
        {
            var result = new ApiPhoneResponse();

            try
            {
                var userPhones = ServicesHost.GetService<IPhonesProvider>().GetUserPhoneNumbers(userId);
                result.Items = userPhones;
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("SaveUserDeviceToken")]
        [HttpPost]
        public JsonResult<ApiBaseResponse> SaveUserDeviceToken([FromUri] Guid userId, string token)
        {
            var result = new ApiBaseResponse();

            try
            {
                var userEntity = UserManager.FindById(userId);

                if (userEntity == null)
                {
                    result.Status = Core.Enums.ApiStatusCode.WrongArgumentsOrData;
                }
                else
                {
                    var device = ServicesHost.GetService<IDevicesProvider>().GetByToken(token);

                    if (device == null)
                    {
                        device = new ApiDevice
                        {
                            Token = token,
                            UserId = userEntity.Id
                        };
                        ServicesHost.GetService<IDevicesProvider>().Save(device);
                    }
                    else if (device.UserId != userEntity.Id)
                    {
                        device.UserId = userEntity.Id;
                        ServicesHost.GetService<IDevicesProvider>().Save(device);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        public JsonResult<ApiUserResponse> Get(Guid id)
        {
            var result = new ApiUserResponse();

            try
            {
                var user = Mapper.Map<OntextUser, ApiUser>(UserManager.FindById(id));
                result.Items = new[] { user }.ToList();
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("GetUserSettings")]
        public JsonResult<ApiSettingsResponse> GetUserSettings(Guid userId)
        {
            var result = new ApiSettingsResponse();

            try
            {
                var userSettings = ServicesHost.GetService<ISettingsProvider>().GetByUserId(userId);
                result.Items = new List<ApiSettings> {userSettings};
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }

        [Route("SaveUserSettings")]
        public JsonResult<ApiBaseResponse> SaveUserSettings([FromBody]ApiSettings settings)
        {
            var result = new ApiBaseResponse();

            try
            {
                ServicesHost.GetService<ISettingsProvider>().Save(settings);
            }
            catch (Exception ex)
            {
                result.Status = Core.Enums.ApiStatusCode.SystemError;
                result.Error = ex.Message;
            }

            return Json(result);
        }
    }
}
