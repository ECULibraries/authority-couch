using System.Collections.Generic;

namespace AuthorityCouch.Models
{
    public class GuideViewModel
    {
        public SearchViewModel Name { get; set; }
        public SearchViewModel Subject { get; set; }

        public List<AgentGroup> AsAgents { get; set; }
        public List<SubjectGroup> AsSubjects { get; set; }
    }
}