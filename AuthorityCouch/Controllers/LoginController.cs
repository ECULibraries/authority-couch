using System.Web.Mvc;

namespace AuthorityCouch.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Redirect("https://lib.ecu.edu/central/Login?ReturnTo=authority&ReturnUrl=" + Request.QueryString["ReturnUrl"]);
        }
    }
}