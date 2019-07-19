using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class ExportController : BaseController
    {
        public ActionResult Name()
        {
            var vm = new ExportViewModel();

            var resources = AsRepo.GetArchivesSpaceResources();

            vm.AsResources = resources.Select(x => new SelectListItem
            {
                Value = $"{ConfigurationManager.AppSettings["ArchivesSpaceUrl"]}{x.id}",
                Text = x.title + $" ({x.ead_id})"
            });

            return View(vm);
        }

        public ActionResult NameAuthorities()
        {
            var results = GetNameAllDocs();

            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,archivesSpaceUri");

            foreach (var result in results)
            {
                if (!result.doc._id.StartsWith("_"))
                {
                    csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri}");
                }
            }

            System.IO.File.WriteAllText(Server.MapPath("~/Download/nameAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/nameAuths.csv");
        }

        [HttpPost]
        public ActionResult NameAuthoritiesByGuide(ExportViewModel evm)
        {
            
            var resources = AsRepo.GetArchivesSpaceResources();
            var found = resources.FirstOrDefault(x => x.title + $" ({x.ead_id})" == evm.NewAsResource);
            var asUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id;
            var requests = SearchNameByAsUri(asUrl);

            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,type");

            var type = string.Empty;
            foreach (var result in requests.Results.Docs)
            {
                if(result.personalNameCreator != null && result.personalNameCreator.Contains(asUrl)) { type = "personalNameCreator"; }
                else if (result.personalNameSource != null && result.personalNameSource.Contains(asUrl)) { type = "personalNameSource"; }
                else if (result.corporateNameCreator != null && result.corporateNameCreator.Contains(asUrl)) { type = "corporateNameCreator"; }
                else if (result.corporateNameSource != null && result.corporateNameSource.Contains(asUrl)) { type = "corporateNameSource"; }
                else if (result.familyNameCreator != null && result.familyNameCreator.Contains(asUrl)) { type = "familyNameCreator"; }
                else if (result.familyNameSource != null && result.familyNameSource.Contains(asUrl)) { type = "familyNameSource"; }
                csv.AppendLine($"{result._id},\"{result.authoritativeLabel}\",{type}");
            }

            System.IO.File.WriteAllText(Server.MapPath($"~/Download/{found.ead_id}NameAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect($"~/Download/{found.ead_id}NameAuths.csv");
        }

        [HttpPost]
        public ActionResult SubjectAuthoritiesByGuide(ExportViewModel evm)
        {

            var resources = AsRepo.GetArchivesSpaceResources();
            var found = resources.FirstOrDefault(x => x.title + $" ({x.ead_id})" == evm.NewAsResource);
            var asUrl = ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id;
            var requests = SearchSubjectByAsUri(asUrl);

            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,type");

            var type = string.Empty;
            foreach (var result in requests.Results.Docs)
            {
                if (result.topic != null && result.topic.Contains(asUrl)) { type = "topic"; }
                else if (result.geographic != null && result.geographic.Contains(asUrl)) { type = "geographic"; }
                else if (result.meeting != null && result.meeting.Contains(asUrl)) { type = "meeting"; }
                else if (result.uniformTitle != null && result.uniformTitle.Contains(asUrl)) { type = "uniformTitle"; }
                else if (result.personalNameSubject != null && result.personalNameSubject.Contains(asUrl)) { type = "personalNameSubject"; }
                else if (result.corporateNameSubject != null && result.corporateNameSubject.Contains(asUrl)) { type = "corporateNameSubject"; }
                else if (result.familyNameSubject != null && result.familyNameSubject.Contains(asUrl)) { type = "familyNameSubject"; }
                csv.AppendLine($"{result._id},\"{result.authoritativeLabel}\",{type}");
            }

            System.IO.File.WriteAllText(Server.MapPath($"~/Download/{found.ead_id}SubjectAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect($"~/Download/{found.ead_id}SubjectAuths.csv");
        }

        public ActionResult Subject()
        {
            var vm = new ExportViewModel();

            var resources = AsRepo.GetArchivesSpaceResources();

            vm.AsResources = resources.Select(x => new SelectListItem
            {
                Value = $"{ConfigurationManager.AppSettings["ArchivesSpaceUrl"]}{x.id}",
                Text = x.title + $" ({x.ead_id})"
            });

            return View(vm);
        }

        public ActionResult SubjectAuthorities()
        {
            var results = GetSubjectAllDocs();

            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,archivesSpaceUri");

            foreach (var result in results)
            {
                if (!result.doc._id.StartsWith("_"))
                {
                    csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri}");
                }
            }

            System.IO.File.WriteAllText(Server.MapPath("~/Download/subjectAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/subjectAuths.csv");
        }
    }
}