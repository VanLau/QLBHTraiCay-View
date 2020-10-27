using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(QLBHTraiCayMVC_View.Startup))]
namespace QLBHTraiCayMVC_View
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
