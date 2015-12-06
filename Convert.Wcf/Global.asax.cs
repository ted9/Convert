using Autofac;
using Autofac.Integration.Wcf;
using Convert.Infrastruture.Caching;
using Convert.Infrastruture.Logging;
using Convert.Service;
using Convert.Service.Interface;
using Convert.Wcf.Interface;
using Convert.Web.Infrastruture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace Convert.Wcf
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<Log4NetLogger>().As<ILogger>();
            builder.RegisterType<Caching>().As<ICache>().SingleInstance();
            builder.RegisterType<ChequeWritingService>().As<IChequeWritingService>();
            builder.RegisterType<ConvertService>().As<IConvertService>();
            var container = builder.Build(); 
            AutofacHostFactory.Container = container; 
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}