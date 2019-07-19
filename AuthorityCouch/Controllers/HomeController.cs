using System.Web.Mvc;
using AuthorityCouch.Models.Import;

namespace AuthorityCouch.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Name()
        {
            //var meeting = new Meeting();

            //AsRepo.SaveAsSubjects(meeting);

            //AsRepo.CreateSubjectRelationship();

            //ImportTopical();
            //ImportGeographicRelations();
            return View();
        }

        public ActionResult Subject() => View();
    }
}