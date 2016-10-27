using System;
using System.Configuration;

namespace Ontext.Server.Core.Configs
{
    public class OntextSection : ConfigurationSection
    {
        public const string SectionName = "OntextSection";

        public static OntextSection GetSection()
        {
            return (OntextSection)ConfigurationManager.GetSection(SectionName);
        }

        [ConfigurationProperty("ImagesUploadSettings")]
        public ImagesUploadSettingsElement ImagesUploadSettings
        {
            get
            {
                return (ImagesUploadSettingsElement)this["ImagesUploadSettings"];
            }
            set
            {
                this["ImagesUploadSettings"] = value;
            }
        }

        [ConfigurationProperty("TwilioSettings")]
        public TwilioSettingsElement TwilioSettings
        {
            get
            {
                return (TwilioSettingsElement)this["TwilioSettings"];
            }
            set
            {
                this["TwilioSettings"] = value;
            }
        }

        [ConfigurationProperty("SendGridSettings")]
        public SendGridSettingsElement SendGridSettings
        {
            get
            {
                return (SendGridSettingsElement)this["SendGridSettings"];
            }
            set
            {
                this["SendGridSettings"] = value;
            }
        }

        [ConfigurationProperty("ApplicationUserSettings")]
        public ApplicationUserSettingsElement ApplicationUserSettings
        {
            get
            {
                return (ApplicationUserSettingsElement)this["ApplicationUserSettings"];
            }
            set
            {
                this["ApplicationUserSettings"] = value;
            }
        }

        [ConfigurationProperty("TwoFactorAuthenticationSettings")]
        public TwoFactorAuthenticationSettingsElement TwoFactorAuthenticationSettings
        {
            get
            {
                return (TwoFactorAuthenticationSettingsElement)this["TwoFactorAuthenticationSettings"];
            }
            set
            {
                this["TwoFactorAuthenticationSettings"] = value;
            }
        }

        [ConfigurationProperty("MobileClientSettings")]
        public MobileClientSettingsElement MobileClientSettings {
            get
            {
                return (MobileClientSettingsElement)this["MobileClientSettings"];
            }
            set
            {
                this["MobileClientSettings"] = value;
            }
        }
    }

    public class ImagesUploadSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("Directory", IsRequired = true)]
        public String Directory
        {
            get { return (string)this["Directory"]; }
            set { this["Directory"] = value; }
        }

        [ConfigurationProperty("Extension", IsRequired = true)]
        public String Extension
        {
            get { return (string)this["Extension"]; }
            set { this["Extension"] = value; }
        }
    }

    public class MobileClientSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("Id", IsRequired = true)]
        public String Id {
            get { return (string) this["Id"]; }
            set { this["Id"] = value; }
        }
        [ConfigurationProperty("Secret", IsRequired = true)]
        public String Secret {
            get { return (string) this["Secret"]; }
            set { this["Secret"] = value; }
        }
        [ConfigurationProperty("Name", IsRequired = true)]
        public String Name {
            get { return (string) this["Name"]; }
            set { this["Name"] = value; }
        }
        [ConfigurationProperty("ApplicationType", IsRequired = true)]
        public Ontext.Core.Enums.ApplicationTypes ApplicationType {
            get { return (Ontext.Core.Enums.ApplicationTypes) this["ApplicationType"]; }
            set { this["ApplicationType"] = value; }
        }
        [ConfigurationProperty("Active", IsRequired = true)]
        public Boolean Active
        {
            get { return (bool) this["Active"]; } 
            set { this["Active"] = value; }
        }
        [ConfigurationProperty("RefreshTokenLifeTime", IsRequired = true)]
        public Int32 RefreshTokenLifetime {
            get { return (int) this["RefreshTokenLifeTime"]; }
            set { this["RefreshTokenLifeTime"] = value; }
        }
        [ConfigurationProperty("AllowedOrigin", IsRequired = true)]
        public String AllowedOrigin {
            get { return (string) this["AllowedOrigin"]; }
            set { this["AllowedOrigin"] = value; }
        }
    }

    public class TwoFactorAuthenticationSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("PhoneCodeProvider", IsRequired = true)]
        public String PhoneCodeProvider
        {
            get
            {
                return (String)this["PhoneCodeProvider"];
            }
            set
            {
                this["PhoneCodeProvider"] = value;
            }
        }

        [ConfigurationProperty("EmailCodeProvider", IsRequired = true)]
        public String EmailCodeProvider
        {
            get
            {
                return (String)this["EmailCodeProvider"];
            }
            set
            {
                this["EmailCodeProvider"] = value;
            }
        }
    }

    public class ApplicationUserSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("DefaultPassword", IsRequired = true)]
        public String DefaultPassword
        {
            get
            {
                return (String)this["DefaultPassword"];
            }
            set
            {
                this["DefaultPassword"] = value;
            }
        }
    }

    public class SendGridSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("UserName", IsRequired = true)]
        public String UserName
        {
            get
            {
                return (String)this["UserName"];
            }
            set
            {
                this["UserName"] = value;
            }
        }

        [ConfigurationProperty("Email", IsRequired = true)]
        public String Email
        {
            get
            {
                return (String)this["Email"];
            }
            set
            {
                this["Email"] = value;
            }
        }

        [ConfigurationProperty("Password", IsRequired = true)]
        public String Password
        {
            get
            {
                return (String)this["Password"];
            }
            set
            {
                this["Password"] = value;
            }
        }
    }

    public class TwilioSettingsElement : ConfigurationElement
    {
        [ConfigurationProperty("Sid", IsRequired = true)]
        public String Sid
        {
            get
            {
                return (String)this["Sid"];
            }
            set
            {
                this["Sid"] = value;
            }
        }

        [ConfigurationProperty("Token", IsRequired = true)]
        public String Token
        {
            get
            {
                return (String)this["Token"];
            }
            set
            {
                this["Token"] = value;
            }
        }

        [ConfigurationProperty("PhoneNumber", IsRequired = true)]
        public String PhoneNumber
        {
            get
            {
                return (String)this["PhoneNumber"];
            }
            set
            {
                this["PhoneNumber"] = value;
            }
        }
    }
}