using System;
using NPoco;

namespace AuthorityCouch.Models
{
    [TableName("subject_rlshp")]
    [PrimaryKey("id")]
    public class AsSubjectRelationship
    {
        public int id { get; set; }
        public int resource_id { get; set; }
        public int subject_id { get; set; }
        public int aspace_relationship_position { get; set; }
        public DateTime system_mtime { get; set; }
        public DateTime user_mtime { get; set; }
        public int suppressed { get; set; }
    }
}