using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Convert.Service;
using Convert.Web.Infrastruture;
using Owin;
using Autofac;
using System.Reflection;

namespace Convert.WebApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();
            builder.RegisterType<Log4NetLogger>().AsImplementedInterfaces();
            builder.RegisterType<Caching>().AsImplementedInterfaces().SingleInstance();
            builder.RegisterType<ChequeWritingService>().AsImplementedInterfaces();

            var container = builder.Build();
            var dependencyResolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = dependencyResolver;

            app.UseAutofacMiddleware(container);
            app.UseAutofacWebApi(config);

            WebApiConfig.Register(config);
            app.UseWebApi(config);
        }
    }
}
