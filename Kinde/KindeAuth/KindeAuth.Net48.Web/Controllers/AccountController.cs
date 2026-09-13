using KindeAuth.Net48.Web.Models;
using KindeAuth.Net48.Web.Security;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace KindeAuth.Net48.Web.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var auth = HttpContext
                .GetOwinContext()
                .Authentication
                .AuthenticateAsync("Cookies")
                .Result;

            var accessToken = auth?.Properties?.Dictionary["access_token"];
            var identity = User.Identity as ClaimsIdentity;

            var model = new UserModel
            {
                UserID = identity?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Name = identity?.FindFirst("name")?.Value,
                Email = identity.FindFirst(ClaimTypes.Email)?.Value,
                FirstName = identity.FindFirst(ClaimTypes.GivenName)?.Value,
                LastName = identity.FindFirst(ClaimTypes.Surname)?.Value,
            };

            return View(model);
        }

        public ActionResult SignIn()
        {
            return new ChallengeResult("OpenIdConnect", HomeUrl());
        }

        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();

            return RedirectPermanent(HomeUrl());
        }

        [AllowAnonymous]
        public ActionResult SignoutCallbackOidc()
        {
            return RedirectPermanent(HomeUrl());
        }

        private string HomeUrl()
        {
            return Url.Action(nameof(HomeController.Index), "Home");
        }
    }
}
