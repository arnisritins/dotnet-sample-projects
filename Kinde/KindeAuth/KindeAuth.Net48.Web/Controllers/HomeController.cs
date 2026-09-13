using System.Web;
using System.Web.Mvc;

namespace KindeAuth.Net48.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Secret()
        {
            return View();
        }
    }
}
