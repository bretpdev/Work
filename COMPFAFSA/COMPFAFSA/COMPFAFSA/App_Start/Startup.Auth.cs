using COMPFAFSA.DataAccess;
using COMPFAFSA.Models;
using COMPFAFSA.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace COMPFAFSA
{
	public partial class Startup
	{
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<UsersDataAccess>(UsersDataAccess.Create);

            //app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                CookieSecure = CookieSecureOption.Always,
                CookieHttpOnly = true,
                LoginPath = new PathString("/Account/Login"),
                ExpireTimeSpan = new TimeSpan(1, 0, 0),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  

                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(15),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
               
            });
            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            //PublicClientId = "self";
            //OAuthOptions = new OAuthAuthorizationServerOptions
            //{
            //    TokenEndpointPath = new PathString("/Token"),
            //    Provider = new ApplicationOAuthProvider(),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
            //    // TODO In production mode set AllowInsecureHttp = false
            //    AllowInsecureHttp = true
            //};
            //app.UseOAuthAuthorizationServer(new Microsoft.Owin.Security.OAuth.OAuthAuthorizationServerOptions
            //{
            //    AllowInsecureHttp = true,//should be disabled in production
            //    Provider = new ApplicationOAuthProvider(PublicClientId),
            //    TokenEndpointPath = new PathString("/Token")
            //});

            // Enable the application to use bearer tokens to authenticate users
            //app.UseOAuthBearerTokens(OAuthOptions);
        }
    }
}