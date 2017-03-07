using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(MyItems.MobileAppService.Startup))]

namespace MyItems.MobileAppService
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
        }
    }
}