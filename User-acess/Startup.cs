using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Brukertilgang.Startup))]
namespace Brukertilgang
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
