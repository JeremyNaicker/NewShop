using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewShop.WebUI.Startup))]
namespace NewShop.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
