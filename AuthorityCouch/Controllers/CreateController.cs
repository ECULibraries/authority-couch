using System.Web.Mvc;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class CreateController : BaseController
    {
        public ActionResult Name()
        {
            var cvm = new Doc();
            if (Request.QueryString["label"] != null)
            {
                cvm.authoritativeLabel = Request.QueryString["label"] ;
            }
            return View(cvm);
        }

        [HttpPost]
        public ActionResult Name(Doc doc)
        {
            var svm = new SearchViewModel();
            svm.Term = doc.authoritativeLabel.Trim();
            var search = SearchNameByLabel(svm);

            if (search != null && search.Results.Docs.Count > 0)
            {
                TempData["Message"] = $"Existing Authority Found: <a href='assign/{search.Results.Docs[0]._id}'>" +
                                      search.Results.Docs[0]._id + "</a>";
            }
            else
            {
                var response = GetUuids(1);
                doc._id = response.uuids[0];
                SaveNameDoc(doc);
                return RedirectToAction("Name", "Edit", new { id = doc._id });
            }

            return View();
        }

        public ActionResult Subject()
        {
            var cvm = new Doc();
            if (Request.QueryString["label"] != null)
            {
                cvm.authoritativeLabel = Request.QueryString["label"];
            }
            return View(cvm);
        }

        [HttpPost]
        public ActionResult Subject(Doc doc)
        {
            var svm = new SearchViewModel();
            svm.Term = doc.authoritativeLabel.Trim();
            var search = SearchSubjectByLabel(svm);

            if (search != null && search.Results.Docs.Count > 0)
            {
                TempData["Message"] = $"Existing Authority Found: <a href='assign/{search.Results.Docs[0]._id}'>" +
                                      search.Results.Docs[0]._id + "</a>";
            }
            else
            {
                var response = GetUuids(1);
                doc._id = response.uuids[0];
                SaveSubjectDoc(doc);
                return RedirectToAction("Subject", "Edit", new { id = doc._id });
            }

            return View();
        }
    }
}