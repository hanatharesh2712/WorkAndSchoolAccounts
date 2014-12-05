using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace MVC_SSO_AccessGraph
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}