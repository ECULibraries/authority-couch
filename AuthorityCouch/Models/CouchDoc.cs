using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthorityCouch.Models
{
    public class CouchResponse
    {
        public string[] uuids { get; set; }
    }

    public class CouchDocs
    {
        public List<Doc> Docs { get; set; }
    }

    public class AllDocs
    {
        public List<Row> rows { get; set; }
    }
    public class Row
    {
        public string id { get; set; }
        public string key { get; set; }
        public Doc doc { get; set; }
    }

    public class Doc
    {
        public string _id { get; set; }
        public string _rev { get; set; }

        [Required]
        [Display(Name = "Full Authoritative Label")]
        public string authoritativeLabel { get; set; }
        public string externalAuthorityUri { get; set; }

        public List<Substring> substrings { get; set; }

        [Display(Name = "ArchivesSpace URI")]
        public string archivesSpaceUri { get; set; }
        
        public List<string> familyNameCreator { get; set; }
        public List<string> familyNameSource { get; set; }
        public List<string> corporateNameCreator { get; set; }
        public List<string> corporateNameSource { get; set; }
        public List<string> personalNameCreator { get; set; }
        public List<string> personalNameSource { get; set; }

        public List<string> topic { get; set; }
        public List<string> geographic { get; set; }
        public List<string> meeting { get; set; }
        public List<string> uniformTitle { get; set; }
        public List<string> personalNameSubject { get; set; }
        public List<string> familyNameSubject { get; set; }
        public List<string> corporateNameSubject { get; set; }

        public List<DcEntry> dcName { get; set; }
        public List<DcEntry> dcSubject { get; set; }

        public bool CanBeDeleted()
        {
            if (topic == null && geographic == null && familyNameCreator == null && familyNameSource == null && familyNameSubject == null &&
                corporateNameCreator == null && corporateNameSource == null && corporateNameSubject == null &&
                personalNameCreator == null && personalNameSource == null && personalNameSubject == null && meeting == null && uniformTitle == null && dcName == null && dcSubject == null)
            {
                return true;
            }

            return false;
        }
    }

    public class Substring
    {
        public string authoritativeLabel { get; set; }
        public string externalAuthorityUri { get; set; }
        public Substring() { }
        public Substring(string label, string uri)
        {
            authoritativeLabel = label;
            externalAuthorityUri = uri;
        }
    }

    public class DcEntry
    {
        public string type { get; set; }
        public string relator_uri { get; set; }
        public string relator_label { get; set; }
        public string uri { get; set; }
    }
}