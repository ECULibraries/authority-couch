using System.Web.Mvc;
using System.Web.Routing;

namespace AuthorityCouch
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.LowercaseUrls = true;

            routes.MapRoute(
                name: "AssignAsName",
                url: "assign/assignnameasresource/",
                defaults: new { controller = "Assign", action = "AssignNameAsResource" }
            );
            routes.MapRoute(
                name: "AssignAsSubject",
                url: "assign/assignsubjectasresource/",
                defaults: new { controller = "Assign", action = "AssignSubjectAsResource" }
            );

            routes.MapRoute(
                name: "Assign",
                url: "assign/{id}",
                defaults: new { controller = "Assign", action = "Index" }
            );

            routes.MapRoute(
                name: "Edit",
                url: "edit/{id}",
                defaults: new { controller = "Edit", action = "Index" }
            );

            routes.MapRoute(
                name: "Subject/Home",
                url: "subject",
                defaults: new { controller = "Home", action = "Subject" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Name", id = UrlParameter.Optional }
            );
        }
    }
}
