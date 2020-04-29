using System.Collections.Generic;
using System.Web.Mvc;

namespace AuthorityCouch.Models
{
    public class EditViewModel
    {
        public Doc Doc { get; set; }
        public string NewLabel { get; set; }
        public string NewUri { get; set; }
        public List<SelectListItem> Types { get; set; }
        //public string SelectedType { get; set; }
        public EditViewModel()
        {
            Types = new List<SelectListItem>();
            Types.Add(new SelectListItem() { Text = "Personal", Value = "Personal" });
            Types.Add(new SelectListItem() { Text = "Family", Value = "Family" });
            Types.Add(new SelectListItem() { Text = "Corporate", Value = "Corporate" });
        }

    }
}