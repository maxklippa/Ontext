using System;
using System.Configuration;
using System.Linq;
using System.Web;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Ontext.BLL.Identity;
using Ontext.BLL.Providers.Contracts;
using Ontext.Core.Objects;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.DAL.Repositories.Contracts;
using Ontext.DAL.UnitOfWork.Contracts;
using Ontext.Server.Core.Settings;

namespace Ontext.BLL.Providers
{
    public class AccountsProvider : IAccountsProvider
    {
        #region [ Private Fields ]

        private readonly IEntityRepository<Phone> _phoneRepository;

        private readonly OntextUserManager _userManager;

        #endregion // [ Private Fields ]

        #region [ Constructors ]

        public AccountsProvider(IUnitOfWork unitOfWork, OntextUserManager userManager)
        {
            if (userManager == null) throw new NullReferenceException("userManager");

            _userManager = userManager;

            _phoneRepository = unitOfWork.GetRepository<Phone>();
        }

        #endregion // [ Constructors ]

        #region [ Public Methods ]

//        public ApiUser GetById(Guid id)
//        {
//            var user = _userManager.FindById(id); 
//
//            return Mapper.Map<OntextUser, ApiUser>(user);
//        }

//        public ApiUser GetByPhoneNumber(string phoneNumber)
//        {
//            var user = _userManager.FindByPhoneNumber(phoneNumber);
//
//            return Mapper.Map<OntextUser, ApiUser>(user);
//        }

//        public ApiUser GetByEmailAddress(string emailAddress)
//        {
//            var user = _userManager.FindByEmail(emailAddress);
//
//            return Mapper.Map<OntextUser, ApiUser>(user);
//        }

//        public void SendTwoFactorTokenBySms(Guid userId, string phoneNumber)
//        {
//            var token = _userManager.GenerateTwoFactorToken(userId, OntextSettings.PhoneTwoFactorProvider);
//
//            _userManager.SendSms(phoneNumber, token);
//        }
//
//        public void SendTwoFactorTokenByEmail(Guid userId, string email)
//        {
//            var token = _userManager.GenerateTwoFactorToken(userId, OntextSettings.PhoneTwoFactorProvider);
//
//            _userManager.SendEmail(email, "Security Code", token);
//        }

//        public bool VerifyTwoFactorToken(Guid userId, string token)
//        {
//            var provider = OntextSettings.PhoneTwoFactorProvider;
//
//            return _userManager.VerifyTwoFactorToken(userId, provider, token);
//        }

        public bool Create(string phoneNumber)
        {
            var userEntity = new OntextUser();

            var phone = _phoneRepository.GetAll().FirstOrDefault(p => p.Number == phoneNumber) ?? new Phone
            {
                Number = phoneNumber
            };

            userEntity.UserName = Guid.NewGuid().ToString();
            userEntity.Phones = new[] { phone };
            userEntity.TwoFactorEnabled = true;

            var userPassword = OntextSettings.UserDefaultPassword;

            var result = _userManager.Create(userEntity, userPassword);

            return result.Succeeded;
        }

        public bool Update(ApiUser user)
        {
            var userEntity = Mapper.Map<ApiUser, OntextUser>(user);

            var result = _userManager.Update(userEntity);

            return result.Succeeded;
        }

        #endregion // [ Public Methods ]
    }
}