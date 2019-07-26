using System.Web.Mvc;
using AuthorityCouch.Models.Import;

namespace AuthorityCouch.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Name()
        {

            //AddAuths();

            return View();
        }

        public ActionResult Subject() => View();
    }
}