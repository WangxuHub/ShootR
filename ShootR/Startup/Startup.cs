using System;
using System.Configuration;
using System.Security.Claims;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.QQ;
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

            app.UseQQConnectAuthentication(new QQConnectAuthenticationOptions
            {
                AppId = ConfigurationManager.AppSettings["qqAppId"],
                AppSecret = ConfigurationManager.AppSettings["qqAppSecret"],
                SignInAsAuthenticationType = "ShootR",
                Provider = new QQConnectAuthenticationProvider
                {
                    onAuthenticated = async context =>
                    {
                        try
                        {
                            var ProfileImageUrl = context.User.Value<string>("figureurl_qq_2");
                            context.Identity.AddClaim(new Claim("profilePicture", ProfileImageUrl));
                        }
                        catch { }
                    }
                }
            });

            //app.UseTwitterAuthentication(new TwitterAuthenticationOptions
            //{
            //    Provider = new TwitterAuthenticationProvider
            //    {
            //        OnAuthenticated = async context =>
            //        {
            //            var service = new TwitterService(ConfigurationManager.AppSettings["twitterConsumerKey"], ConfigurationManager.AppSettings["twitterConsumerSecret"]);
            //            service.AuthenticateWith(context.AccessToken, context.AccessTokenSecret);

            //            var profile = service.GetUserProfile(new GetUserProfileOptions
            //            {
            //                IncludeEntities = true
            //            });

            //            context.Identity.AddClaim(new Claim("profilePicture", profile.ProfileImageUrl));
            //        }
            //    },
            //    SignInAsAuthenticationType = "ShootR",
            //    ConsumerKey = ConfigurationManager.AppSettings["twitterConsumerKey"],
            //    ConsumerSecret = ConfigurationManager.AppSettings["twitterConsumerSecret"]
            //});
        }
    }
}