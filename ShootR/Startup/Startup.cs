using System;
using System.Configuration;
using System.Security.Claims;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.Cookies;
using Owin;
using ShootR.Authentication;
using TweetSharp;

namespace ShootR
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();

            // Disable keep alive, no need
            GlobalHost.Configuration.DisconnectTimeout = TimeSpan.FromMinutes(3);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = "ShootR",
                Provider = new ShootRAuthenticationProvider()
            });
        }
    }
}