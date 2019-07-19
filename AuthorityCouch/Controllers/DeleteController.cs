using System.Web.Mvc;

namespace AuthorityCouch.Controllers
{
    public class DeleteController : BaseController
    {
        public ActionResult Name(string id)
        {
            DeleteNameDoc(id);
            return RedirectToAction("Name", "Home");
        }

        public ActionResult NameByPosition(string id, int position)
        {
            var doc = GetNameDocByUuid(id);
            if (position == 1)
            {
                doc.substrings = null;
            }
            else
            {
                doc.substrings.RemoveAt(position-1);
            }

            SaveNameDoc(doc);

            return RedirectToAction("Name", "Edit", new { id });
        }

        public ActionResult Subject(string id)
        {
            DeleteSubjectDoc(id);
            return RedirectToAction("Subject", "Home");
        }

        public ActionResult SubjectByPosition(string id, int position)
        {
            var doc = GetSubjectDocByUuid(id);
            if (position == 1)
            {
                doc.substrings = null;
            }
            else
            {
                doc.substrings.RemoveAt(position - 1);
            }

            SaveSubjectDoc(doc);

            return RedirectToAction("Subject", "Edit", new { id });
        }
    }
}