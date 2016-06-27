using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MatchGame.Startup))]
namespace MatchGame
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
