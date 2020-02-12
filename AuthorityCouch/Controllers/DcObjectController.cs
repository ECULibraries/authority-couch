using System.Configuration;
using System.Web.Mvc;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class DcObjectController : BaseController
    {
        public ActionResult NameView(int id)
        {
            ViewBag.DigitalCollectionsUrl = ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + id;
            var dvm = new DcObjectViewModel();
            dvm.Name = SearchNameByDcUri(ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + id);

            //var gvm = new GuideViewModel();
            //var resources = AsRepo.GetArchivesSpaceResources();

            //gvm.Name = SearchNameByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);
            //gvm.Subject = SearchSubjectByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);

            //var match = resources.FirstOrDefault(x => x.id == id);
            //ViewBag.Guide = match.title + $" ({match.ead_id})";

            //return View(gvm);

            return View(dvm);
        }

        public ActionResult SubjectView(int id)
        {
            ViewBag.DigitalCollectionsUrl = ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + id;
            var dvm = new DcObjectViewModel();
            dvm.Name = SearchSubjectByDcUri(ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + id);

            //var gvm = new GuideViewModel();
            //var resources = AsRepo.GetArchivesSpaceResources();

            //gvm.Name = SearchNameByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);
            //gvm.Subject = SearchSubjectByAsUri(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + id);

            //var match = resources.FirstOrDefault(x => x.id == id);
            //ViewBag.Guide = match.title + $" ({match.ead_id})";

            //return View(gvm);

            return View(dvm);
        }
    }
}