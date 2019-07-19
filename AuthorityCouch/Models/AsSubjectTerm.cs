using NPoco;

namespace AuthorityCouch.Models
{
    [TableName("subject_term")]
    [PrimaryKey("id")]
    public class AsSubjectTerm
    {
        public int id { get; set; }
        public int subject_id { get; set; }
        public int term_id { get; set; }
    }
}