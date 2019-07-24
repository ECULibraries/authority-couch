using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AuthorityCouch.App_Start.Startup))]
namespace AuthorityCouch.App_Start
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}