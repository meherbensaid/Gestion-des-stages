using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GestionDesStages.Startup))]
namespace GestionDesStages
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
