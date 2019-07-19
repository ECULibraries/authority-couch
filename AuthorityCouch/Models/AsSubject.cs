using System;
using NPoco;

namespace AuthorityCouch.Models
{
    [TableName("subject")]
    [PrimaryKey("id")]
    public class AsSubject
    {
        public int id { get; set; }
        public int lock_version { get; set; }
        public int json_schema_version { get; set; }
        public int vocab_id { get; set; }
        public string title { get; set; }
        public string terms_sha1 { get; set; }
        public string authority_id { get; set; }
        public int source_id { get; set; }
        public string created_by { get; set; }
        public string last_modified_by { get; set; }
        public DateTime create_time { get; set; }
        public DateTime system_mtime { get; set; }
        public DateTime user_mtime { get; set; }
    }
}