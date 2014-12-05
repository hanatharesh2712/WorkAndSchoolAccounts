using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Configuration;
using System.Globalization;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.IdentityModel.Claims;
using System.Threading.Tasks;
using Owin;
using MVC_SSO_AccessGraph.Models;

namespace MVC_SSO_AccessGraph
{
    public partial class Startup
    {
        private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private static string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private static string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
        private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

        public static readonly string Authority = String.Format(CultureInfo.InvariantCulture, aadInstance, tenantId);

        // This is the resource ID of the AAD Graph API.  We'll need this to request a token to call the Graph API.
        // Generic mode for picking up this setting
        string graphResourceId = "https://graph.windows.net";

        public void ConfigureAuth(IAppBuilder app)
        {
            TenantDbContext db = new TenantDbContext();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            app.UseOpenIdConnectAuthentication(
                new OpenIdConnectAuthenticationOptions
                {
                    ClientId = clientId,
                    Authority = Authority,
                    PostLogoutRedirectUri = postLogoutRedirectUri,

                    Notifications = new OpenIdConnectAuthenticationNotifications()
                    {
                        //
                        // If there is a code in the OpenID Connect response, redeem it for an access token and refresh token, and store those away.
                        //

                       AuthorizationCodeReceived = (context) =>
                       {
                           var code = context.Code;
                           ClientCredential credential = new ClientCredential(clientId, appKey);
                           string userObjectID = context.AuthenticationTicket.Identity.FindFirst(
                               "http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
                           AuthenticationContext authContext = new AuthenticationContext(Authority, new EFADALTokenCache(userObjectID));
                           AuthenticationResult result = authContext.AcquireTokenByAuthorizationCode(
                           code, new Uri(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path)), credential, graphResourceId);

                           return Task.FromResult(0);
                       }
                    }
                });
        }
    }
}