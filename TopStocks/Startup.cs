using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TopStocks.Startup))]
namespace TopStocks
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
