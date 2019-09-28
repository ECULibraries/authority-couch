using System.Collections.Generic;
using System.Web.Mvc;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class EditController : BaseController
    {
        public ActionResult Name(string id)
        {
            var vm = new EditViewModel();
            vm.Doc = GetNameDocByUuid(id);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Name(EditViewModel evm)
        {
            var svm = new SearchViewModel();
            svm.Term = evm.Doc.authoritativeLabel;
            var search = SearchNameByLabel(svm);

            // if one doc with same id -- no change
            // if one doc with different id -- another auth has this value
            if (search.Results.Docs.Count == 1)
            {
                if (search.Results.Docs[0]._id != evm.Doc._id)
                {
                    TempData["Message"] = $"No changes made: existing authority found with provided label: <a href='" + 
                                          Url.Action("Name", new { id = search.Results.Docs[0]._id }) + "'>" +
                                          search.Results.Docs[0]._id + "</a>";
                    return RedirectToAction("Name", new { id = evm.Doc._id });
                }
            }
            // if more than one -- another auth and a problem
            else if (search.Results.Docs.Count > 1) 
            {
                TempData["Message"] = $"No changes made: existing authority found with provided label: <a href='" +
                                      Url.Action("Name", new { id = search.Results.Docs[0]._id }) + "'>" +
                                      search.Results.Docs[0]._id + "</a>";
                return RedirectToAction("Name", new { id = evm.Doc._id });
            }
            
            // if zero -- update 
            var fullDoc = GetNameDocByUuid(evm.Doc._id);
            fullDoc.authoritativeLabel = evm.Doc.authoritativeLabel;
            fullDoc.externalAuthorityUri = evm.Doc.externalAuthorityUri;
            fullDoc.archivesSpaceUri = evm.Doc.archivesSpaceUri;

            // wipe substrings if externalAuthorityUri has value
            if (fullDoc.externalAuthorityUri != null)
            {
                fullDoc.substrings = null;
            }
            else 
            {
                fullDoc.substrings = evm.Doc.substrings;

                if (evm.NewLabel != null && evm.NewUri != null)
                { 
                    if (fullDoc.substrings == null) { fullDoc.substrings = new List<Substring>();}
                    fullDoc.substrings.Add(new Substring(evm.NewLabel, evm.NewUri));
                }
            }

            SaveNameDoc(fullDoc);

            return RedirectToAction("Name", new { id = evm.Doc._id });
        }

        public ActionResult Subject(string id)
        {
            var vm = new EditViewModel();
            vm.Doc = GetSubjectDocByUuid(id);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Subject(EditViewModel evm)
        {
            var svm = new SearchViewModel();
            svm.Term = evm.Doc.authoritativeLabel;
            var search = SearchSubjectByLabel(svm);

            // if one doc with same id -- no change
            // if one doc with different id -- another auth has this value
            if (search.Results.Docs.Count == 1)
            {
                if (search.Results.Docs[0]._id != evm.Doc._id)
                {
                    TempData["Message"] = $"No changes made: existing authority found with provided label: <a href='" +
                                          Url.Action("Name", new { id = search.Results.Docs[0]._id }) + "'>" +
                                          search.Results.Docs[0]._id + "</a>";
                    return RedirectToAction("Name", new { id = evm.Doc._id });
                }
            }
            // if more than one -- another auth and a problem
            else if (search.Results.Docs.Count > 1)
            {
                TempData["Message"] = $"No changes made: existing authority found with provided label: <a href='" +
                                      Url.Action("Name", new { id = search.Results.Docs[0]._id }) + "'>" +
                                      search.Results.Docs[0]._id + "</a>";
                return RedirectToAction("Name", new { id = evm.Doc._id });
            }

            // if zero -- update 
            var fullDoc = GetSubjectDocByUuid(evm.Doc._id);
            fullDoc.authoritativeLabel = evm.Doc.authoritativeLabel;
            fullDoc.externalAuthorityUri = evm.Doc.externalAuthorityUri;
            fullDoc.archivesSpaceUri = evm.Doc.archivesSpaceUri;

            // wipe substrings if externalAuthorityUri has value
            if (fullDoc.externalAuthorityUri != null)
            {
                fullDoc.substrings = null;
            }
            else
            {
                fullDoc.substrings = evm.Doc.substrings;

                if (evm.NewLabel != null) //&& evm.NewUri != null
                {
                    if (fullDoc.substrings == null) { fullDoc.substrings = new List<Substring>(); }
                    fullDoc.substrings.Add(new Substring(evm.NewLabel, evm.NewUri));
                }
            }

            SaveSubjectDoc(fullDoc);

            return RedirectToAction("Subject", new { id = evm.Doc._id });
        }
    }
}