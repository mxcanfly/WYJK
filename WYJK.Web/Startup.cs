using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WYJK.Web.Startup))]
namespace WYJK.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
