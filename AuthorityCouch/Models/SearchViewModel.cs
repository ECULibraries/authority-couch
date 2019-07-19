namespace AuthorityCouch.Models
{
    public class SearchViewModel
    {
        public string Term { get; set; }
        public CouchDocs Results { get; set; }
    }
}