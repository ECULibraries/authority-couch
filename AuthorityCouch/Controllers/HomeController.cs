using System.Collections.Generic;
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

            return View();
        }

        public ActionResult Subject() => View();
    }
}