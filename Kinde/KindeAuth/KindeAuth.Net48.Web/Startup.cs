using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Configuration;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(KindeAuth.Net48.Web.Startup))]
namespace KindeAuth.Net48.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType
            });

            app.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType,

                SaveTokens = true,
                RedeemCode = true,
                ResponseType = "code",
                Scope = "openid profile email",

                ClientId = ConfigurationManager.AppSettings["Kinde:ClientId"],
                ClientSecret = ConfigurationManager.AppSettings["Kinde:ClientSecret"],
                Authority = ConfigurationManager.AppSettings["Kinde:Authority"],
                RedirectUri = ConfigurationManager.AppSettings["Kinde:RedirectUri"],
                PostLogoutRedirectUri = ConfigurationManager.AppSettings["Kinde:SignOutUri"],

                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    RedirectToIdentityProvider = context =>
                    {
                        return Task.FromResult(0);
                    },

                    AuthorizationCodeReceived = context =>
                    {
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
