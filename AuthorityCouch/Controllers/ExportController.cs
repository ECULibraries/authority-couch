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
            var agentLinks = AsRepo.GetLinkedAgents();

            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,archivesSpaceUri,rid,found");

            AgentGroup found;
            var match = false;

            foreach (var result in results)
            {
                if (!result.doc._id.StartsWith("_"))
                {
                    if (result.doc.personalNameCreator != null)
                    {
                        foreach (var item in result.doc.personalNameCreator)
                        {
                            found = agentLinks.Find(x => x.person_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 878);
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},personalCreator,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.personalNameSource != null)
                    {
                        foreach (var item in result.doc.personalNameSource)
                        {
                            found = agentLinks.Find(x => x.person_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 879);
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},personalSource,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.familyNameCreator != null)
                    {
                        foreach (var item in result.doc.familyNameCreator)
                        {
                            found = agentLinks.Find(x => x.family_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 878);
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},familyCreator,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.familyNameSource != null)
                    {
                        foreach (var item in result.doc.familyNameSource)
                        {
                            found = agentLinks.Find(x => x.family_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 879);
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},familySource,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.corporateNameCreator != null)
                    {
                        foreach (var item in result.doc.corporateNameCreator)
                        {
                            found = agentLinks.Find(x => x.corp_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 878);
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},corporateCreator,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.corporateNameSource != null)
                    {
                        foreach (var item in result.doc.corporateNameSource)
                        {
                            found = agentLinks.Find(x => x.corp_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 879);
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},corporateSource,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
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

        public ActionResult AsNameReport()
        {
            var agentLinks = AsRepo.GetLinkedAgents();
            var authority = GetNameAllDocs();

            var csv = new StringBuilder();

            csv.AppendLine("found,as_link,r_link,resource_id,role_id,agent_person_id,person_name,agent_family_id,family_name,agent_corporate_entity_id,corp_name");

            foreach (var link in agentLinks)
            {
                Row match;
                bool labelMatch, asLink, rLink;
                if (link.agent_corporate_entity_id != null)
                {
                    match = authority.Find(x => x.doc.authoritativeLabel == link.corp_name);
                    if (match == null)
                    {
                        labelMatch = false;
                        asLink = false;
                        rLink = false;
                    }
                    else
                    {
                        labelMatch = true;
                        asLink = link.agent_corporate_entity_id.ToString() == match.doc.archivesSpaceUri.Replace("http://archivesspace.ecu.edu/agents/agent_corporate_entity/", "");
                        if (link.role_id == 878)
                        {
                            rLink = match.doc.corporateNameCreator.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                        }
                        else
                        {
                            rLink = match.doc.corporateNameSource.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                        }
                    }
                }
                else if (link.agent_person_id != null)
                {
                    match = authority.Find(x => x.doc.authoritativeLabel == link.person_name);
                    if (match == null)
                    {
                        labelMatch = false;
                        asLink = false;
                        rLink = false;
                    }
                    else
                    {
                        labelMatch = true;
                        asLink = link.agent_person_id.ToString() == match.doc.archivesSpaceUri.Replace("http://archivesspace.ecu.edu/agents/agent_person/", "");
                        if (link.role_id == 878)
                        {
                            rLink = match.doc.personalNameCreator.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                        }
                        else
                        {
                            rLink = match.doc.personalNameSource.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                        }
                    }
                }
                else
                {
                    match = authority.Find(x => x.doc.authoritativeLabel == link.family_name);
                    if (match == null)
                    {
                        labelMatch = false;
                        asLink = false;
                        rLink = false;
                    }
                    else
                    {
                        labelMatch = true;
                        asLink = link.agent_family_id.ToString() == match.doc.archivesSpaceUri.Replace("http://archivesspace.ecu.edu/agents/agent_family/", "");
                        if (link.role_id == 878)
                        {
                            rLink = match.doc.familyNameCreator.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                        }
                        else
                        {
                            rLink = match.doc.familyNameSource.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                        }
                    }
                }

                csv.AppendLine($"{labelMatch},{asLink},{rLink},{link.resource_id},{link.role_id},{link.agent_person_id},\"{link.person_name}\",{link.agent_family_id},\"{link.family_name}\",{link.agent_corporate_entity_id},\"{link.corp_name}\"");
            }

            System.IO.File.WriteAllText(Server.MapPath("~/Download/AsNameReport.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/AsNameReport.csv");
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
            var authority = GetSubjectAllDocs();
            var subjectLinks = AsRepo.GetLinkedSubjects();
            
            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,archivesSpaceUri,type,rid,found");

            SubjectGroup found;
            var match = false;

            foreach (var result in authority)
            {
                if (!result.doc._id.StartsWith("_"))
                {
                    if (result.doc.topic != null)
                    {
                        foreach (var topic in result.doc.topic)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == topic.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "topical");
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},topical,{topic.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.geographic != null)
                    {
                        foreach (var item in result.doc.geographic)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "geographic");
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},geographic,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.personalNameSubject != null)
                    {
                        foreach (var item in result.doc.personalNameSubject)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "personal");
                            if (found != null){match = true;}
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},personal,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.familyNameSubject != null)
                    {
                        foreach (var item in result.doc.familyNameSubject)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "family");
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},family,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.corporateNameSubject != null)
                    {
                        foreach (var item in result.doc.corporateNameSubject)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "corporate");
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},corporate,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.meeting != null)
                    {
                        foreach (var item in result.doc.meeting)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "meeting");
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},meeting,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                    if (result.doc.uniformTitle != null)
                    {
                        foreach (var item in result.doc.uniformTitle)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "uniform_title");
                            if (found != null) { match = true; }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},title,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match}");
                        }
                    }
                }
            }

            System.IO.File.WriteAllText(Server.MapPath("~/Download/subjectAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/subjectAuths.csv");
        }

        public ActionResult AsSubjectReport()
        {
            var subjectLinks = AsRepo.GetLinkedSubjects();
            var authority = GetSubjectAllDocs();

            var csv = new StringBuilder();

            csv.AppendLine("found,as_link,r_link,sid,rid,subject,type");

            foreach (var link in subjectLinks)
            {
               
                Row match;
                bool labelMatch, asLink, rLink;
                match = authority.Find(x => x.doc.authoritativeLabel == link.subject);
                if (match == null)
                {
                    labelMatch = false;
                    asLink = false;
                    rLink = false;
                }
                else
                {
                    labelMatch = true;
                    asLink = link.id.ToString() == match.doc.archivesSpaceUri.Replace("http://archivesspace.ecu.edu/subjects/", "");
                    if (link.type == "topical")
                    {
                        rLink = match.doc.topic.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                    }
                    else if (link.type == "geographic")
                    {
                        rLink = match.doc.geographic.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                    }
                    else if (link.type == "personal")
                    {
                        rLink = match.doc.personalNameSubject.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                    }
                    else if (link.type == "family")
                    {
                        rLink = match.doc.familyNameSubject.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                    }
                    else if (link.type == "corporate")
                    {
                        rLink = match.doc.corporateNameSubject.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                    }
                    else if (link.type == "meeting")
                    {
                        rLink = match.doc.meeting.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                    }
                    else if (link.type == "uniform_title")
                    {
                        rLink = match.doc.uniformTitle.Contains(ConfigurationManager.AppSettings["ArchivesSpaceUrl"] + link.resource_id);
                    }
                    else
                    {
                        rLink = false;
                    }
                }
                
                csv.AppendLine($"{labelMatch},{asLink},{rLink},{link.id},{link.resource_id},\"{link.subject}\",{link.type}");
            }

            System.IO.File.WriteAllText(Server.MapPath("~/Download/AsSubjectReport.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/AsSubjectReport.csv");
        }
    }
}