using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DPTPWebAPI.Startup))]

namespace DPTPWebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Configure the SignalR hosting    
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
