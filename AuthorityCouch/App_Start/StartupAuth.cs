using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace AuthorityCouch.App_Start
{
    public static class CentralAuthentication
    {
        public const string ApplicationCookie = "CentralAuthenticationType";
    }

    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CentralAuthentication.ApplicationCookie,
                CookieDomain = ".ecu.edu",
                LoginPath = new PathString("/Login"),
                Provider = new CookieAuthenticationProvider(),
                CookieName = "CentralAuthenticationCookie",
                CookieHttpOnly = true,
                ExpireTimeSpan = TimeSpan.FromHours(5)
            });
        }
    }
}