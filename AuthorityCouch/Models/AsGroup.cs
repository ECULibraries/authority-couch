﻿namespace AuthorityCouch.Models
{
    public class AgentGroup
    {
        public int? agent_person_id { get; set; }
        public int? agent_family_id { get; set; }
        public int? agent_corporate_entity_id { get; set; }
        public int role_id { get; set; }
        public int? relator_id { get; set; }
        public string person_name { get; set; }
        public string family_name { get; set; }
        public string corp_name { get; set; }
    }

    public class SubjectGroup
    {
        public string subject { get; set; }
        public string type { get; set; }
        public int id { get; set; }
    }
}