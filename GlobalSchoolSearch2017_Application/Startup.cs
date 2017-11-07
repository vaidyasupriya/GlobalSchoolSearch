using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GlobalSchoolSearch2017_Application.Startup))]
namespace GlobalSchoolSearch2017_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
