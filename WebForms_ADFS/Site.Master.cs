using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.WsFederation;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace WebForms_SSO
{
    public partial class SiteMaster : MasterPage
    {
        public string UserName;
        protected void Page_Load(object sender, EventArgs e)
        {
            var claimsIdentity = Context.User.Identity as System.Security.Claims.ClaimsIdentity;
            if (claimsIdentity != null)
            {
                foreach (System.Security.Claims.Claim claim in claimsIdentity.Claims)
                {
                    string ClaimType = claim.Type;
                    if (ClaimType == "http://schemas.microsoft.com/ws/2008/06/identity/claims/windowsaccountname")
                    {
                        string ClaimValue = claim.Value;
                        UserName = ClaimValue;
                    }
                    
                }
            }
        }
        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            // Redirect to ~/Account/SignOut after signing out.
            string callbackUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Response.ApplyAppPathModifier("~/Account/SignOut");
           
            HttpContext.Current.GetOwinContext().Authentication.SignOut(
                new AuthenticationProperties { RedirectUri = callbackUrl },
                WsFederationAuthenticationDefaults.AuthenticationType,
                CookieAuthenticationDefaults.AuthenticationType);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    WsFederationAuthenticationDefaults.AuthenticationType);
            }
        }
    }
}