using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AuthorityCouch.Models;
using AuthorityCouch.Models.Import;

namespace AuthorityCouch.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Name()
        {
            //ImportEadRelations();
            //ViewData["Result"] = AsRepo.SaveAsSubjects(mtg);
            //var Subs = new List<string>
            //            {
            //                "Alford, Arthur S., 1929-1982--Correspondence"

            //            };
            //var vm = new SearchViewModel();
            //foreach (var sub in Subs)
            //{
            //    vm.Term = sub;
            //    vm = SearchSubjectByLabel(vm);
            //    if (vm.Results.Docs.Count == 0)
            //    {
            //        ViewData["Result"] += sub + "_0^";
            //    }
            //    else
            //    {
            //        ViewData["Result"] += sub + "_" + vm.Results.Docs[0]._id + "^";
            //    }

            //}
            //ImportNameAuths();

            //var nameids = new Personal();
            //foreach (var id in nameids.NewAs)
            //{
            //    var doc = GetNameDocByUuid(id);

            //    if (doc.personalNameCreator != null || doc.personalNameSource != null)
            //    {
            //        doc.type = "Personal";
            //        doc.creator = doc.personalNameCreator;
            //        doc.source = doc.personalNameSource;
            //        doc.personalNameCreator = null;
            //        doc.personalNameSource = null;
            //    }
            //    else if (doc.familyNameCreator != null || doc.familyNameSource != null)
            //    {
            //        doc.type = "Family";
            //        doc.creator = doc.familyNameCreator;
            //        doc.source = doc.familyNameSource;
            //        doc.familyNameCreator = null;
            //        doc.familyNameSource = null;
            //    }
            //    else if (doc.corporateNameCreator != null || doc.corporateNameSource != null)
            //    {
            //        doc.type = "Corporate";
            //        doc.creator = doc.corporateNameCreator;
            //        doc.source = doc.corporateNameSource;
            //        doc.corporateNameCreator = null;
            //        doc.corporateNameSource = null;
            //    }

            //    SaveNameDoc(doc);
            //}

            //var subids = new Personal();
            //foreach (var id in subids.NewAs)
            //{
            //    var doc = GetSubjectDocByUuid(id);

            //    if (doc.topic != null)
            //    {
            //        doc.type = "Topic";
            //        doc.archivesSpaceRelations = doc.topic;
            //        doc.topic = null;
            //    }
            //    else if (doc.geographic != null)
            //    {
            //        doc.type = "Geographic";
            //        doc.archivesSpaceRelations = doc.geographic;
            //        doc.geographic = null;
            //    }
            //    else if (doc.personalNameSubject != null)
            //    {
            //        doc.type = "PersonalName";
            //        doc.archivesSpaceRelations = doc.personalNameSubject;
            //        doc.personalNameSubject = null;
            //    }
            //    else if (doc.familyNameSubject != null)
            //    {
            //        doc.type = "FamilyName";
            //        doc.archivesSpaceRelations = doc.familyNameSubject;
            //        doc.familyNameSubject = null;
            //    }
            //    else if (doc.corporateNameSubject != null)
            //    {
            //        doc.type = "CorporateName";
            //        doc.archivesSpaceRelations = doc.corporateNameSubject;
            //        doc.corporateNameSubject = null;
            //    }
            //    else if (doc.meeting != null)
            //    {
            //        doc.type = "Meeting";
            //        doc.archivesSpaceRelations = doc.meeting;
            //        doc.meeting = null;
            //    }
            //    else if (doc.uniformTitle != null)
            //    {
            //        doc.type = "UniformTitle";
            //        doc.archivesSpaceRelations = doc.uniformTitle;
            //        doc.uniformTitle = null;
            //    }

            //    SaveSubjectDoc(doc);
            //}

            return View();
        }

        public ActionResult Subject() => View();
    }
}