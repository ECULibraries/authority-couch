using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AuthorityCouch.Models;

namespace AuthorityCouch.Controllers
{
    public class ExportController : BaseController
    {
        // AS Name
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
            csv.AppendLine("id,authoritativeLabel,externalAuthorityUri,archivesSpaceUri,type,rid,found,auth_id_exists");

            foreach (var result in results)
            {
                AgentGroup found;
                var match = false;
                var authMatch = false;

                if (!result.doc._id.StartsWith("_"))
                {
                    var externalUri = string.Empty;
                    if (result.doc.externalAuthorityUri != null)
                    {
                        externalUri = result.doc.externalAuthorityUri;
                    }
                    else if (result.doc.substrings != null && result.doc.substrings.Count > 0)
                    {
                        externalUri = string.Join(";", result.doc.substrings?.Select(x => x.externalAuthorityUri));
                    }
                    
                    if (result.doc.personalNameCreator != null)
                    {
                        foreach (var item in result.doc.personalNameCreator)
                        {
                            found = agentLinks.Find(x => x.person_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 878);
                            if (found != null)
                            {
                                match = true;
                                authMatch = result.doc._id == found.authority_id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},personalCreator,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.personalNameSource != null)
                    {
                        foreach (var item in result.doc.personalNameSource)
                        {
                            found = agentLinks.Find(x => x.person_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 879);
                            if (found != null)
                            {
                                match = true;
                                authMatch = result.doc._id == found.authority_id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},personalSource,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.familyNameCreator != null)
                    {
                        foreach (var item in result.doc.familyNameCreator)
                        {
                            found = agentLinks.Find(x => x.family_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 878);
                            if (found != null)
                            {
                                match = true;
                                authMatch = result.doc._id == found.authority_id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},familyCreator,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.familyNameSource != null)
                    {
                        foreach (var item in result.doc.familyNameSource)
                        {
                            found = agentLinks.Find(x => x.family_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 879);
                            if (found != null)
                            {
                                match = true;
                                authMatch = result.doc._id == found.authority_id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},familySource,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.corporateNameCreator != null)
                    {
                        foreach (var item in result.doc.corporateNameCreator)
                        {
                            found = agentLinks.Find(x => x.corp_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 878);
                            if (found != null)
                            {
                                match = true;
                                authMatch = result.doc._id == found.authority_id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},corporateCreator,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.corporateNameSource != null)
                    {
                        foreach (var item in result.doc.corporateNameSource)
                        {
                            found = agentLinks.Find(x => x.corp_name == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.role_id == 879);
                            if (found != null)
                            {
                                match = true;
                                authMatch = result.doc._id == found.authority_id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},corporateSource,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }

                    if (result.doc.personalNameCreator == null && result.doc.personalNameSource == null &&
                        result.doc.familyNameCreator == null && result.doc.familyNameSource == null &&
                        result.doc.corporateNameCreator == null && result.doc.corporateNameSource == null)
                    {
                        csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{result.doc.archivesSpaceUri},corporateSource,FALSE,{match},{authMatch}");
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

            csv.AppendLine("resource_id,repo,role_id,agent_person_id,person_name,agent_family_id,family_name,agent_corporate_entity_id,corp_name,found,auth_id_exists");

            foreach (var link in agentLinks)
            {
                Row found = null;
                var match = false;
                var authMatch = false;
                var repo = link.repo_id == 2 ? "Archives" : link.repo_id == 3 ? "Manuscripts" : "Laupus";
                if (link.agent_corporate_entity_id != null)
                {
                    if (link.role_id == 878)
                    {
                        found = authority.Find(x => x.doc.authoritativeLabel == link.corp_name && x.doc.corporateNameCreator != null && x.doc.corporateNameCreator.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && x.doc.archivesSpaceUri == "http://archivesspace.ecu.edu/agents/agent_corporate_entity/" + link.agent_corporate_entity_id);
                    }
                    else if(link.role_id == 879)
                    {
                        found = authority.Find(x => x.doc.authoritativeLabel == link.corp_name && x.doc.corporateNameSource != null && x.doc.corporateNameSource.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && x.doc.archivesSpaceUri == "http://archivesspace.ecu.edu/agents/agent_corporate_entity/" + link.agent_corporate_entity_id);
                    }

                    if (found != null)
                    {
                        match = true;
                        authMatch = found.doc._id == link.authority_id;
                    }
                    csv.AppendLine($"{link.resource_id},{repo},{link.role_id},{link.agent_person_id},\"{link.person_name}\",{link.agent_family_id},\"{link.family_name}\",{link.agent_corporate_entity_id},\"{link.corp_name}\",{match},{authMatch}");
                }
                else if (link.agent_person_id != null)
                {
                    if (link.role_id == 878)
                    {
                        found = authority.Find(x => x.doc.authoritativeLabel == link.person_name && x.doc.personalNameCreator != null && x.doc.personalNameCreator.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && x.doc.archivesSpaceUri == "http://archivesspace.ecu.edu/agents/agent_person/" + link.agent_person_id);
                    }
                    else if (link.role_id == 879)
                    {
                        found = authority.Find(x => x.doc.authoritativeLabel == link.person_name && x.doc.personalNameSource != null && x.doc.personalNameSource.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && x.doc.archivesSpaceUri == "http://archivesspace.ecu.edu/agents/agent_person/" + link.agent_person_id);
                    }

                    if (found != null)
                    {
                        match = true;
                        authMatch = found.doc._id == link.authority_id;
                    }
                    csv.AppendLine($"{link.resource_id},{repo},{link.role_id},{link.agent_person_id},\"{link.person_name}\",{link.agent_family_id},\"{link.family_name}\",{link.agent_corporate_entity_id},\"{link.corp_name}\",{match},{authMatch}");
                }
                else if(link.agent_family_id != null)
                {
                    if (link.role_id == 878)
                    {
                        found = authority.Find(x => x.doc.authoritativeLabel == link.family_name && x.doc.familyNameCreator != null && x.doc.familyNameCreator.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && x.doc.archivesSpaceUri == "http://archivesspace.ecu.edu/agents/agent_family/" + link.agent_family_id);
                    }
                    else if (link.role_id == 879)
                    {
                        found = authority.Find(x => x.doc.authoritativeLabel == link.family_name && x.doc.familyNameSource != null && x.doc.familyNameSource.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && x.doc.archivesSpaceUri == "http://archivesspace.ecu.edu/agents/agent_family/" + link.agent_family_id);
                    }

                    if (found != null)
                    {
                        match = true;
                        authMatch = found.doc._id == link.authority_id;
                    }
                    csv.AppendLine($"{link.resource_id},{repo},{link.role_id},{link.agent_person_id},\"{link.person_name}\",{link.agent_family_id},\"{link.family_name}\",{link.agent_corporate_entity_id},\"{link.corp_name}\",{match},{authMatch}");
                }
            }

            System.IO.File.WriteAllText(Server.MapPath("~/Download/AsNameReport.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/AsNameReport.csv");
        }

        // DC Name

        public ActionResult DcNameAuthorities()
        {
            var results = GetDcNameAllDocs();
            
            var csv = new StringBuilder();
            csv.AppendLine("id,pid,authoritativeLabel,type,relator_label,relator_uri");

            foreach (var result in results.Docs)
            {
                foreach(var name in result.dcName)
                {
                    csv.AppendLine($"{result._id},{name.uri.Replace(ConfigurationManager.AppSettings["DigitalCollectionsUrl"], "")},\"{result.authoritativeLabel}\",{name.type},{name.relator_label},{name.relator_uri}");
                }
            }
            System.IO.File.WriteAllText(Server.MapPath("~/Download/dcNameAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/dcNameAuths.csv");
        }

        [HttpPost]
        public ActionResult NameAuthoritiesByPid(ExportViewModel evm)
        {
            var dcUrl = ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + evm.DcPid;
            var requests = SearchNameByDcUri(dcUrl);

            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,type,relator_label,relator_uri");

            var type = string.Empty;
            foreach (var result in requests.Results.Docs)
            {
                foreach (var name in result.dcName.FindAll(x => x.uri == dcUrl))
                {
                    csv.AppendLine($"{result._id},\"{result.authoritativeLabel}\",{name.type},{name.relator_label},{name.relator_uri}");
                }
            }

            System.IO.File.WriteAllText(Server.MapPath($"~/Download/{evm.DcPid}_NameAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect($"~/Download/{evm.DcPid}_NameAuths.csv");
        }

        // AS Subject

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
            csv.AppendLine("id,authoritativeLabel,externalAuthorityUri,archivesSpaceUri,type,rid,found,auth_id_exists");

            SubjectGroup found;
            
            foreach (var result in authority)
            {
                var match = false;
                var authMatch = false;

                if (!result.doc._id.StartsWith("_"))
                {
                    var externalUri = string.Empty;
                    if (result.doc.externalAuthorityUri != null)
                    {
                        externalUri = result.doc.externalAuthorityUri;
                    }
                    else if (result.doc.substrings != null && result.doc.substrings.Count > 0)
                    {
                        externalUri = string.Join(";", result.doc.substrings?.Select(x => x.externalAuthorityUri));
                    }

                    if (result.doc.topic != null)
                    {
                        foreach (var topic in result.doc.topic)
                        {
                            if (result.doc.authoritativeLabel.StartsWith("Inchon"))
                            {
                                var x = 33;
                            }
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == topic.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "topical");
                            if (found != null)
                            {
                                match = true;
                                authMatch = found.authority_id == result.doc._id;
                            }
                            
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},topical,{topic.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.geographic != null)
                    {
                        foreach (var item in result.doc.geographic)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "geographic");
                            if (found != null)
                            {
                                match = true;
                                authMatch = found.authority_id == result.doc._id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},geographic,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.personalNameSubject != null)
                    {
                        foreach (var item in result.doc.personalNameSubject)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "personal");
                            if (found != null)
                            {
                                match = true;
                                authMatch = found.authority_id == result.doc._id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},personal,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.familyNameSubject != null)
                    {
                        foreach (var item in result.doc.familyNameSubject)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "family");
                            if (found != null)
                            {
                                match = true;
                                authMatch = found.authority_id == result.doc._id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},family,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.corporateNameSubject != null)
                    {
                        foreach (var item in result.doc.corporateNameSubject)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "corporate");
                            if (found != null)
                            {
                                match = true;
                                authMatch = found.authority_id == result.doc._id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},corporate,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.meeting != null)
                    {
                        foreach (var item in result.doc.meeting)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "meeting");
                            if (found != null)
                            {
                                match = true;
                                authMatch = found.authority_id == result.doc._id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},meeting,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
                        }
                    }
                    if (result.doc.uniformTitle != null)
                    {
                        foreach (var item in result.doc.uniformTitle)
                        {
                            found = subjectLinks.Find(x => x.subject == result.doc.authoritativeLabel && x.resource_id.ToString() == item.Replace("http://archivesspace.ecu.edu/resources/", "") && x.type == "uniform_title");
                            if (found != null)
                            {
                                match = true;
                                authMatch = found.authority_id == result.doc._id;
                            }
                            csv.AppendLine($"{result.doc._id},\"{result.doc.authoritativeLabel}\",{externalUri},{result.doc.archivesSpaceUri},title,{item.Replace("http://archivesspace.ecu.edu/resources/", "")},{match},{authMatch}");
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

            csv.AppendLine("sid,rid,repo,subject,type,found,auth_id_exists");

            foreach (var link in subjectLinks)
            {
               
                Row found;
                var match = false;
                var authMatch = false;
                var repo = link.repo_id == 2 ? "Archives" : link.repo_id == 3 ? "Manuscripts" : "Laupus";
                if (link.type == "topical")
                {
                    found = authority.Find(x => x.doc.authoritativeLabel == link.subject && x.doc.topic.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && link.type == "topical");
                    if (found != null)
                    {
                        match = true;
                        authMatch = link.authority_id == found.doc._id;
                    }
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
                else if (link.type == "geographic")
                {
                    found = authority.Find(x => x.doc.authoritativeLabel == link.subject && x.doc.geographic.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && link.type == "geographic");
                    if (found != null)
                    {
                        match = true;
                        authMatch = link.authority_id == found.doc._id;
                    }
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
                else if (link.type == "personal")
                {
                    found = authority.Find(x => x.doc.authoritativeLabel == link.subject && x.doc.personalNameSubject.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && link.type == "personal");
                    if (found != null)
                    {
                        match = true;
                        authMatch = link.authority_id == found.doc._id;
                    }
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
                else if (link.type == "family")
                {
                    found = authority.Find(x => x.doc.authoritativeLabel == link.subject && x.doc.familyNameSubject.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && link.type == "family");
                    if (found != null)
                    {
                        match = true;
                        authMatch = link.authority_id == found.doc._id;
                    }
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
                else if (link.type == "corporate")
                {
                    found = authority.Find(x => x.doc.authoritativeLabel == link.subject && x.doc.corporateNameSubject.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && link.type == "corporate");
                    if (found != null)
                    {
                        match = true;
                        authMatch = link.authority_id == found.doc._id;
                    }
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
                else if (link.type == "meeting")
                {
                    found = authority.Find(x => x.doc.authoritativeLabel == link.subject && x.doc.meeting.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && link.type == "meeting");
                    if (found != null)
                    {
                        match = true;
                        authMatch = link.authority_id == found.doc._id;
                    }
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
                else if (link.type == "uniform_title")
                {
                    found = authority.Find(x => x.doc.authoritativeLabel == link.subject && x.doc.uniformTitle.Contains("http://archivesspace.ecu.edu/resources/" + link.resource_id) && link.type == "uniform_title");
                    if (found != null)
                    {
                        match = true;
                        authMatch = link.authority_id == found.doc._id;
                    }
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
                else
                {
                    csv.AppendLine($"{link.id},{link.resource_id},{repo},\"{link.subject}\",{link.type},{match},{authMatch}");
                }
            }

            System.IO.File.WriteAllText(Server.MapPath("~/Download/AsSubjectReport.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/AsSubjectReport.csv");
        }

        // DC Subject

        public ActionResult DcSubjectAuthorities()
        {
            var results = GetDcSubjectAllDocs();

            var csv = new StringBuilder();
            csv.AppendLine("id,pid,authoritativeLabel,type");

            foreach (var result in results.Docs)
            {
                foreach (var name in result.dcName)
                {
                    csv.AppendLine($"{result._id},{name.uri.Replace("http://digital.lib.ecu.edu/", "")},\"{result.authoritativeLabel}\",{name.type}");
                }
            }
            System.IO.File.WriteAllText(Server.MapPath("~/Download/dcSubjectAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect("~/Download/dcSubjectAuths.csv");
        }

        [HttpPost]
        public ActionResult SubjectAuthoritiesByPid(ExportViewModel evm)
        {
            var dcUrl = ConfigurationManager.AppSettings["DigitalCollectionsUrl"] + evm.DcPid;
            var requests = SearchSubjectByDcUri(dcUrl);

            var csv = new StringBuilder();
            csv.AppendLine("id,authoritativeLabel,type");

            var type = string.Empty;
            foreach (var result in requests.Results.Docs)
            {
                foreach (var name in result.dcSubject.FindAll(x => x.uri == dcUrl))
                {
                    csv.AppendLine($"{result._id},\"{result.authoritativeLabel}\",{name.type}");
                }
            }

            System.IO.File.WriteAllText(Server.MapPath($"~/Download/{evm.DcPid}_SubjectAuths.csv"), csv.ToString(), Encoding.UTF8);
            return Redirect($"~/Download/{evm.DcPid}_SubjectAuths.csv");
        }
    }
}