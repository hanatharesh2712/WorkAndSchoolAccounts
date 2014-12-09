using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace WebForms_MultiOrg_AccessGraph
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}