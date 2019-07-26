using System.Web.Mvc;
using AuthorityCouch.Attributes;

namespace AuthorityCouch
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthorizeAttribute());
            //filters.Add(new CentralAuthorizeAttribute("Administrators"));
        }
    }
}
