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
        public List<SelectListItem> SubjectTypes { get; set; }
        public EditViewModel()
        {
            Types = new List<SelectListItem>();
            Types.Add(new SelectListItem() { Text = "Personal", Value = "Personal" });
            Types.Add(new SelectListItem() { Text = "Family", Value = "Family" });
            Types.Add(new SelectListItem() { Text = "Corporate", Value = "Corporate" });

            SubjectTypes = new List<SelectListItem>();
            SubjectTypes.Add(new SelectListItem() { Text = "Topic", Value = "Topic" });
            SubjectTypes.Add(new SelectListItem() { Text = "Geographic", Value = "Geographic" });
            SubjectTypes.Add(new SelectListItem() { Text = "PersonalName", Value = "PersonalName" });
            SubjectTypes.Add(new SelectListItem() { Text = "FamilyName", Value = "FamilyName" });
            SubjectTypes.Add(new SelectListItem() { Text = "CorporateName", Value = "CorporateName" });
            SubjectTypes.Add(new SelectListItem() { Text = "Meeting", Value = "Meeting" });
            SubjectTypes.Add(new SelectListItem() { Text = "UniformTitle", Value = "UniformTitle" });
        }

    }
}