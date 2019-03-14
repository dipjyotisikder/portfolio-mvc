using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(folio.Startup))]
namespace folio
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
