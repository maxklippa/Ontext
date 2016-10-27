using System.Runtime.InteropServices;
using Ontext.Core.Enums;
using Ontext.Server.Core.Configs;

namespace Ontext.Server.Core.Settings
{
    public static class OntextSettings
    {
        private static readonly OntextSection Section = OntextSection.GetSection();
        private static readonly TwilioSettingsElement TwilioSettings = Section.TwilioSettings;
        private static readonly SendGridSettingsElement SendGridSettings = Section.SendGridSettings;
        private static readonly TwoFactorAuthenticationSettingsElement TwoFactorAuthenticationSettings = Section.TwoFactorAuthenticationSettings;
        private static readonly ApplicationUserSettingsElement ApplicationUserSettings = Section.ApplicationUserSettings;
        private static readonly MobileClientSettingsElement MobileClientSettings = Section.MobileClientSettings;
        private static readonly ImagesUploadSettingsElement ImagesUploadSettings = Section.ImagesUploadSettings;

        public static readonly string UploadImageDirectoryPath = ImagesUploadSettings.Directory;
        public static readonly string UploadImageExtension = ImagesUploadSettings.Extension;

        public static readonly string TwilioSid = TwilioSettings.Sid;
        public static readonly string TwilioToken = TwilioSettings.Token;
        public static readonly string TwilioPhoneNumber = TwilioSettings.PhoneNumber;

        public static readonly string SendGridUserName = SendGridSettings.UserName;
        public static readonly string SendGridEmail = SendGridSettings.Email;
        public static readonly string SendGridPassword = SendGridSettings.Password;

        public static readonly string PhoneTwoFactorProvider = TwoFactorAuthenticationSettings.PhoneCodeProvider;
        public static readonly string EmailTwoFactorProvider = TwoFactorAuthenticationSettings.EmailCodeProvider;

        public static readonly string UserDefaultPassword = ApplicationUserSettings.DefaultPassword;

        public static readonly string MobileClientId = MobileClientSettings.Id;
        public static readonly string MobileClientSecret = MobileClientSettings.Secret;
        public static readonly string MobileClientName = MobileClientSettings.Name;
        public static readonly ApplicationTypes MobileClientApplicationType = MobileClientSettings.ApplicationType;
        public static readonly bool MobileClientActive = MobileClientSettings.Active;
        public static readonly int MobileClientRefreshTokenLifeTime = MobileClientSettings.RefreshTokenLifetime;
        public static readonly string MobileClientAllowedOrigin = MobileClientSettings.AllowedOrigin;
    }
}