using NPoco;

namespace AuthorityCouch.Models
{
    [TableName("resource")]
    [PrimaryKey("id")]
    public class AsResource
    {
        public int id { get; set; }
        public string title { get; set; }
        public string ead_id { get; set; }
        public int repo_id { get; set; }
    }
}