using System.Web;
using System.Web.Mvc;
using KindeAuth.Net48.Web.Security;

namespace KindeAuth.Net48.Web.Controllers
{
    public class AccountController : Controller
    {
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
