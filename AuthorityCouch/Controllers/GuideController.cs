using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AuthorityCouch.Helpers;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class GuideController : BaseController
    {
        public ActionResult Name()
        {
            ViewBag.ArchivesSpaceUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"];
            return View(AsRepo.GetArchivesSpaceResources());
        }

        public ActionResult NameView(int id)
        {
            ViewBag.ArchivesSpaceUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id;
            var gvm = new GuideViewModel();
            var resources = AsRepo.GetArchivesSpaceResources();

            gvm.Name = SearchNameByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);
            gvm.Subject = SearchSubjectByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);

            var match = resources.FirstOrDefault(x => x.id == id);
            ViewBag.Guide = match.title + $" ({match.ead_id})";

            return View(gvm);
        }

        public ActionResult NameCheck(int id)
        {
            ViewBag.ArchivesSpaceUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id;
            var gvm = AsRepo.GetResourceAuthorities(id);
            var resources = AsRepo.GetArchivesSpaceResources();

            gvm.Name = SearchNameByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);
            gvm.Subject = SearchSubjectByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);

            var match = resources.FirstOrDefault(x => x.id == id);
            ViewBag.Guide = match.title + $" ({match.ead_id})";

            return View(gvm);
        }

        public ActionResult Subject()
        {
            ViewBag.ArchivesSpaceUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"];
            return View(AsRepo.GetArchivesSpaceResources());
        }

        public ActionResult SubjectView(int id)
        {
            ViewBag.ArchivesSpaceUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id;
            var gvm = new GuideViewModel();
            var resources = AsRepo.GetArchivesSpaceResources();

            gvm.Name = SearchNameByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);
            gvm.Subject = SearchSubjectByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);

            var match = resources.FirstOrDefault(x => x.id == id);
            ViewBag.Guide = match.title + $" ({match.ead_id})";

            return View(gvm);
        }

        //public ActionResult Tester()
        //{
        //    var resources = AsRepo.GetArchivesSpaceResources();
        //    //var count = 0;
        //    //for (var i = 0; i < 100; i++)
        //    foreach (var guide in resources)
        //    {
        //        var subs = SearchSubjectByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + guide.id);
        //        foreach (var sub in subs.Results.Docs)
        //        {
        //            ViewBag.Results += $"{guide.ead_id}*{sub.authoritativeLabel}*";
        //        }

        //        ViewBag.Results += "$$";
        //        //count++;
        //        //if (count == 5) break;
        //    }
        //    return View();
        //}

        public ActionResult SubjectCheck(int id)
        {
            ViewBag.ArchivesSpaceUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id;
            var gvm = AsRepo.GetResourceAuthorities(id);
            var resources = AsRepo.GetArchivesSpaceResources();

            gvm.Name = SearchNameByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);
            gvm.Subject = SearchSubjectByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);

            var match = resources.FirstOrDefault(x => x.id == id);
            ViewBag.Guide = match.title + $" ({match.ead_id})";

            return View(gvm);
        }

        public ActionResult UpdateNameCache()
        {
            CacheHelper.Clear("archivesspace_cache");
            return RedirectToAction("Name");
        }

        public ActionResult UpdateSubjectCache()
        {
            CacheHelper.Clear("archivesspace_cache");
            return RedirectToAction("Subject");
        }
    }
}