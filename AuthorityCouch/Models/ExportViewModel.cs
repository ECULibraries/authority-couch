using System.Collections.Generic;
using System.Web.Mvc;

namespace AuthorityCouch.Models
{
    public class ExportViewModel
    {
        public IEnumerable<SelectListItem> AsResources { get; set; }
        public string NewAsResource { get; set; }
        public string DcPid { get; set; }
    }
}