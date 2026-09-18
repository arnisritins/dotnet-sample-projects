using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Newtonsoft.Json;
using Owin;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
                AuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                CookieName = "PortalAuth"
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
                    AuthenticationFailed = n =>
                    {
                        return Task.FromResult(0);
                    },

                    AuthorizationCodeReceived = n =>
                    {
                        return Task.FromResult(0);
                    },

                    SecurityTokenReceived = n =>
                    {
                        return Task.FromResult(0);
                    },

                    SecurityTokenValidated = n =>
                    {
                        var accessToken = n.OwinContext.Get<string>("access_token");

                        var jwt = new JwtSecurityTokenHandler()
                            .ReadJwtToken(accessToken);

                        var identity = n.AuthenticationTicket.Identity;

                        foreach (var roleClaim in jwt.Claims.Where(c => c.Type == "roles"))
                        {
                            dynamic role = JsonConvert.DeserializeObject(roleClaim.Value);
                            identity.AddClaim(new Claim(ClaimTypes.Role, role.key.Value));
                        }

                        return Task.FromResult(0);
                    },

                    TokenResponseReceived = n =>
                    {
                        var accessToken = n.TokenEndpointResponse.AccessToken;

                        n.OwinContext.Set("access_token", accessToken);
                        
                        return Task.FromResult(0);
                    }
                }
            });
        }
    }
}
