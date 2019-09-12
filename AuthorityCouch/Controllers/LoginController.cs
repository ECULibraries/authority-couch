using System.Configuration;
using System.Web.Mvc;

namespace AuthorityCouch.Controllers
{
    public class LoginController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return Redirect(ConfigurationManager.AppSettings["LoginUrl"] + Request.QueryString["ReturnUrl"]);
        }
    }
}