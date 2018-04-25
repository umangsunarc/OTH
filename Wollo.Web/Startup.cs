﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Wollo.Web.Startup))]
namespace Wollo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
