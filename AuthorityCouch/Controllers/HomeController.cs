using System.Web.Mvc;

namespace AuthorityCouch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}