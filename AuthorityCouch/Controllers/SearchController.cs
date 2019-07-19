using System.Linq;
using System.Web;
using System.Web.Mvc;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class SearchController : BaseController
    {
        public ActionResult Name()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public ActionResult Name(string term)
        {
            var svm = new SearchViewModel();
            svm.Term = term.First().ToString().ToUpper() + term.Trim().Substring(1);

            var docs = SearchNamePrefix(svm);

            ViewBag.EncodedTerm = HttpUtility.HtmlEncode(svm.Term);

            return View(docs);
        }

        public ActionResult Subject()
        {
            return View(new SearchViewModel());
        }

        [HttpPost]
        public ActionResult Subject(string term)
        {
            var svm = new SearchViewModel();
            svm.Term = term.First().ToString().ToUpper() + term.Trim().Substring(1);

            var docs = SearchSubjectPrefix(svm);

            ViewBag.EncodedTerm = HttpUtility.HtmlEncode(svm.Term);

            return View(docs);
        }
    }
}