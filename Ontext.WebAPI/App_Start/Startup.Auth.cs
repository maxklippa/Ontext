using System;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dependencies;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Ontext.BLL;
using Ontext.BLL.Identity;
using Ontext.DAL;
using Ontext.DAL.Context;
using Ontext.DAL.Context.Contracts;
using Ontext.DAL.Identity;
using Ontext.DAL.Models;
using Ontext.DAL.UnitOfWork;
using Ontext.DAL.UnitOfWork.Contracts;
using Owin;
using Ontext.WebAPI.Providers;

namespace Ontext.WebAPI
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
//            app.CreatePerOwinContext(() => (IOntextDbContext)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IOntextDbContext)));
            app.CreatePerOwinContext(() => (IOntextDbContext)(new OntextDbContext()));

            app.CreatePerOwinContext<IRoleStore<OntextRole, Guid>>((options, context) =>
                new OntextRoleStore(context.Get<IOntextDbContext>()));
            app.CreatePerOwinContext<IUserStore<OntextUser, Guid>>((options, context) =>
                new OntextUserStore(context.Get<IOntextDbContext>()));

            app.CreatePerOwinContext<OntextUserManager>((options, context) =>
            {
                var manager = new OntextUserManager(context.Get<IUserStore<OntextUser, Guid>>());
                
                var dataProtectionProvider = options.DataProtectionProvider;
                if (dataProtectionProvider != null)
                {
                    manager.UserTokenProvider = new TotpSecurityStampBasedTokenProvider<OntextUser, Guid>();
                }
                return manager;
            });

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";

            app.CreatePerOwinContext(() => (IUnitOfWork)GlobalConfiguration.Configuration.DependencyResolver.GetService(typeof(IUnitOfWork)));
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                AllowInsecureHttp = true,
                RefreshTokenProvider = new ApplicationRefreshTokenProvider()
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
