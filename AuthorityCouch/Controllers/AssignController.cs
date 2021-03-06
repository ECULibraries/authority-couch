﻿using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class AssignController : BaseController
    {
        public ActionResult Name(string id)
        {
            var avm = new AssignViewModel();

            avm.Doc = GetNameDocByUuid(id);

            var resources = AsRepo.GetArchivesSpaceResources();

            avm.AsResources = resources.Select(x => new SelectListItem
            {
                Value = $"{ConfigurationManager.AppSettings["ArchivesSpaceUrl"]}{x.id}",
                Text = x.title + $" ({x.ead_id})"
            });

            return View(avm);
        }

        [HttpPost]
        public ActionResult AssignNameAsResource(AssignViewModel avm, string asButton)
        {
            asButton = Regex.Replace(asButton, @" \(...\)", "");
            var resources = AsRepo.GetArchivesSpaceResources();
            var found = resources.FirstOrDefault(x => x.title + $" ({x.ead_id})" == avm.NewAsResource);

            if (found == null)
            {
                TempData["Message"] = "Resource Not Found";
            }
            else
            {
                var fullDoc = GetNameDocByUuid(avm.Doc._id);
                if (asButton == "Creator")
                {
                    if (fullDoc.creator == null) { fullDoc.creator = new List<string>(); }
                    fullDoc.creator.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                }
                else if (asButton == "Source")
                {
                    if (fullDoc.source == null) { fullDoc.source = new List<string>(); }
                    fullDoc.source.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                }

                //if (asButton == "PersonalNameCreator")
                //{
                //    if (fullDoc.personalNameCreator == null) { fullDoc.personalNameCreator = new List<string>(); }
                //    fullDoc.personalNameCreator.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "PersonalNameSource")
                //{
                //    if (fullDoc.personalNameSource == null) { fullDoc.personalNameSource = new List<string>(); }
                //    fullDoc.personalNameSource.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "FamilyNameCreator")
                //{
                //    if (fullDoc.familyNameCreator == null) { fullDoc.familyNameCreator = new List<string>(); }
                //    fullDoc.familyNameCreator.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "FamilyNameSource")
                //{
                //    if (fullDoc.familyNameSource == null) { fullDoc.familyNameSource = new List<string>(); }
                //    fullDoc.familyNameSource.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "CorporateNameCreator")
                //{
                //    if (fullDoc.corporateNameCreator == null) { fullDoc.corporateNameCreator = new List<string>(); }
                //    fullDoc.corporateNameCreator.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "CorporateNameSource")
                //{
                //    if (fullDoc.corporateNameSource == null) { fullDoc.corporateNameSource = new List<string>(); }
                //    fullDoc.corporateNameSource.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}

                SaveNameDoc(fullDoc);
                TempData["Message"] = "Resource Added";
            }

            return RedirectToAction("Name", new { id = avm.Doc._id });
        }

        public ActionResult RemoveNameAsResource(string id, string type, string uri)
        {
            var fullDoc = GetNameDocByUuid(id);
            switch (type)
            {
                case "creator":
                    fullDoc.creator.Remove(uri);
                    if (fullDoc.creator.Count == 0) { fullDoc.creator = null; }
                    break;
                case "source":
                    fullDoc.source.Remove(uri);
                    if (fullDoc.source.Count == 0) { fullDoc.source = null; }
                    break;
                //case "personalNameCreator":
                //    fullDoc.personalNameCreator.Remove(uri);
                //    if (fullDoc.personalNameCreator.Count == 0) { fullDoc.personalNameCreator = null; }
                //    break;
                //case "personalNameSource":
                //    fullDoc.personalNameSource.Remove(uri);
                //    if (fullDoc.personalNameSource.Count == 0) { fullDoc.personalNameSource = null; }
                //    break;
                //case "familyNameCreator":
                //    fullDoc.familyNameCreator.Remove(uri);
                //    if (fullDoc.familyNameCreator.Count == 0) { fullDoc.familyNameCreator = null; }
                //    break;
                //case "familyNameSource":
                //    fullDoc.familyNameSource.Remove(uri);
                //    if (fullDoc.familyNameSource.Count == 0) { fullDoc.familyNameSource = null; }
                //    break;
                //case "corporateNameCreator":
                //    fullDoc.corporateNameCreator.Remove(uri);
                //    if (fullDoc.corporateNameCreator.Count == 0) { fullDoc.corporateNameCreator = null; }
                //    break;
                //case "corporateNameSource":
                //    fullDoc.corporateNameSource.Remove(uri);
                //    if (fullDoc.corporateNameSource.Count == 0) { fullDoc.corporateNameSource = null; }
                //    break;
            }
            SaveNameDoc(fullDoc);
            TempData["Message"] = "ArchivesSpace Resource Removed";
            return RedirectToAction("Name", new { id });

        }

        [HttpPost]
        public ActionResult AssignDcName(AssignViewModel avm, string dcButton)
        {
            int parsedPid;
            if (avm.NewDcResource == null)
            {
                TempData["Message"] = "No PID supplied";
            }
            else if (!int.TryParse(avm.NewDcResource, out parsedPid))
            {
                TempData["Message"] = "PID must be a number";
            }
            else
            {
                var fullDoc = GetNameDocByUuid(avm.Doc._id);
                if (fullDoc.dcName == null) { fullDoc.dcName = new List<DcEntry>(); }

                var entry = new DcEntry { type = dcButton, uri = ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + avm.NewDcResource };

                if(avm.RelatorLabel != null && avm.RelatorUri != null)
                {
                    entry.relator_uri = avm.RelatorUri;
                    entry.relator_label = avm.RelatorLabel;
                }

                fullDoc.dcName.Add(entry);

                SaveNameDoc(fullDoc);
                TempData["Message"] = "Resource Added";
            }

            return RedirectToAction("Name", new { id = avm.Doc._id });
        }

        public ActionResult RemoveNameDcResource(string id, string type, string uri)
        {
            var fullDoc = GetNameDocByUuid(id);
            fullDoc.dcName.Remove(fullDoc.dcName.Find(x => x.type == type && x.uri == uri));
            if(fullDoc.dcName.Count == 0) {  fullDoc.dcName = null; }

            SaveNameDoc(fullDoc);
            TempData["Message"] = "DC Resource Removed";
            return RedirectToAction("Name", new { id });

        }

        //

        public ActionResult Subject(string id)
        {
            var avm = new AssignViewModel();

            avm.Doc = GetSubjectDocByUuid(id);

            var resources = AsRepo.GetArchivesSpaceResources();

            avm.AsResources = resources.Select(x => new SelectListItem
            {
                Value = $"{ConfigurationManager.AppSettings["ArchivesSpaceUrl"]}{x.id}",
                Text = x.title + $" ({x.ead_id})"
            });

            return View(avm);
        }

        [HttpPost]
        public ActionResult AssignSubjectAsResource(AssignViewModel avm, string asButton)
        {
            asButton = Regex.Replace(asButton, @" \(...\)", "");
            var resources = AsRepo.GetArchivesSpaceResources();
            var found = resources.FirstOrDefault(x => x.title + $" ({x.ead_id})" == avm.NewAsResource);

            if (found == null)
            {
                TempData["Message"] = "Resource Not Found";
            }
            else
            {
                var fullDoc = GetSubjectDocByUuid(avm.Doc._id);
                if (asButton == "Assign")
                {
                    if (fullDoc.archivesSpaceRelations == null) { fullDoc.archivesSpaceRelations = new List<string>(); }
                    fullDoc.archivesSpaceRelations.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                }
                //if (asButton == "Topic")
                //{
                //    if (fullDoc.topic == null) { fullDoc.topic = new List<string>(); }
                //    fullDoc.topic.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "Geographic")
                //{
                //    if (fullDoc.geographic == null) { fullDoc.geographic = new List<string>(); }
                //    fullDoc.geographic.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "PersonalName")
                //{
                //    if (fullDoc.personalNameSubject == null) { fullDoc.personalNameSubject = new List<string>(); }
                //    fullDoc.personalNameSubject.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "FamilyName")
                //{
                //    if (fullDoc.familyNameSubject == null) { fullDoc.familyNameSubject = new List<string>(); }
                //    fullDoc.familyNameSubject.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "CorporateName")
                //{
                //    if (fullDoc.corporateNameSubject == null) { fullDoc.corporateNameSubject = new List<string>(); }
                //    fullDoc.corporateNameSubject.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "MeetingName")
                //{
                //    if (fullDoc.meeting == null) { fullDoc.meeting = new List<string>(); }
                //    fullDoc.meeting.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}
                //else if (asButton == "UniformTitle")
                //{
                //    if (fullDoc.uniformTitle == null) { fullDoc.uniformTitle = new List<string>(); }
                //    fullDoc.uniformTitle.Add(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + found.id);
                //}

                SaveSubjectDoc(fullDoc);
                TempData["Message"] = "Resource Added";
            }

            return RedirectToAction("Subject", new { id = avm.Doc._id });
        }

        public ActionResult RemoveSubjectAsResource(string id, string type, string uri)
        {
            var fullDoc = GetSubjectDocByUuid(id);

            fullDoc.archivesSpaceRelations.Remove(uri);
            if (fullDoc.archivesSpaceRelations.Count == 0) { fullDoc.topic = null; }
            //switch (type)
            //{
            //    case "topic":
            //        fullDoc.topic.Remove(uri);
            //        if (fullDoc.topic.Count == 0) { fullDoc.topic = null; }
            //        break;
            //    case "geographic":
            //        fullDoc.geographic.Remove(uri);
            //        if (fullDoc.geographic.Count == 0) { fullDoc.geographic = null; }
            //        break;
            //    case "personalNameSubject":
            //        fullDoc.personalNameSubject.Remove(uri);
            //        if (fullDoc.personalNameSubject.Count == 0) { fullDoc.personalNameSubject = null; }
            //        break;
            //    case "familyNameSubject":
            //        fullDoc.familyNameSubject.Remove(uri);
            //        if (fullDoc.familyNameSubject.Count == 0) { fullDoc.familyNameSubject = null; }
            //        break;
            //    case "corporateNameSubject":
            //        fullDoc.corporateNameSubject.Remove(uri);
            //        if (fullDoc.corporateNameSubject.Count == 0) { fullDoc.corporateNameSubject = null; }
            //        break;
            //    case "meeting":
            //        fullDoc.meeting.Remove(uri);
            //        if (fullDoc.meeting.Count == 0) { fullDoc.meeting = null; }
            //        break;
            //    case "uniformTitle":
            //        fullDoc.uniformTitle.Remove(uri);
            //        if (fullDoc.uniformTitle.Count == 0) { fullDoc.uniformTitle = null; }
            //        break;
            //}
            SaveSubjectDoc(fullDoc);
            TempData["Message"] = "ArchivesSpace Resource Removed";
            return RedirectToAction("Subject", new { id });
        }

        [HttpPost]
        public ActionResult AssignDcSubject(AssignViewModel avm, string dcButton)
        {
            dcButton = Regex.Replace(dcButton, @" \(...\)", "");
            int parsedPid;
            if (avm.NewDcResource == null)
            {
                TempData["Message"] = "No PID supplied";
            }
            else if (!int.TryParse(avm.NewDcResource, out parsedPid))
            {
                TempData["Message"] = "PID must be a number";
            }
            else
            {
                var fullDoc = GetSubjectDocByUuid(avm.Doc._id);
                if (fullDoc.dcSubject == null) { fullDoc.dcSubject = new List<DcEntry>(); }

                var entry = new DcEntry { type = dcButton, uri = ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + avm.NewDcResource };

                fullDoc.dcSubject.Add(entry);

                SaveSubjectDoc(fullDoc);
                TempData["Message"] = "Resource Added";
            }

            return RedirectToAction("Subject", new { id = avm.Doc._id });
        }

        public ActionResult RemoveSubjectDcResource(string id, string type, string uri)
        {
            var fullDoc = GetSubjectDocByUuid(id);
            fullDoc.dcSubject.Remove(fullDoc.dcSubject.Find(x => x.type == type && x.uri == uri));
            if (fullDoc.dcSubject.Count == 0) { fullDoc.dcSubject = null; }

            SaveSubjectDoc(fullDoc);
            TempData["Message"] = "DC Resource Removed";
            return RedirectToAction("Subject", new { id });

        }
    }
}