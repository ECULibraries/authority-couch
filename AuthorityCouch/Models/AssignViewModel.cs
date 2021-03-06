﻿using System.Collections.Generic;
using System.Web.Mvc;

namespace AuthorityCouch.Models
{
    public class AssignViewModel
    {
        public Doc Doc { get; set; }
        public IEnumerable<SelectListItem> AsResources { get; set; }
        public string NewAsResource { get; set; }
        public string NewDcResource { get; set; }
        public string RelatorUri { get; set; }
        public string RelatorLabel { get; set; }
    }
}