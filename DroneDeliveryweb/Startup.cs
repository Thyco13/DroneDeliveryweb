using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DroneDeliveryweb.Startup))]

namespace DroneDeliveryweb
{
    public partial class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}