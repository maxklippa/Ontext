using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Ontext.WebAPI.Startup))]

namespace Ontext.WebAPI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
