using System.Web.Mvc;
using AuthorityCouch.Models.Import;

namespace AuthorityCouch.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Name()
        {

            //ImportNameAuths();

            return View();
        }

        public ActionResult Subject() => View();
    }
}