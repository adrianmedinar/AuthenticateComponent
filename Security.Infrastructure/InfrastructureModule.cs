using Autofac;
using Axity.Security.Core.Interfaces.Gateways.Repositories;
using Axity.Security.Core.Interfaces.Services;
using Axity.Security.Infrastructure.Auth;
using Axity.Security.Infrastructure.Data.Repositories;
using Axity.Security.Infrastructure.Interfaces;
using Axity.Security.Infrastructure.Logging;
using Module = Autofac.Module;

namespace Axity.Security.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
            builder.RegisterType<JwtTokenHandler>().As<IJwtTokenHandler>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
            builder.RegisterType<TokenFactory>().As<ITokenFactory>().SingleInstance();
            builder.RegisterType<JwtTokenValidator>().As<IJwtTokenValidator>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
            builder.RegisterType<Logger>().As<ILogger>().SingleInstance();
        }
    }
}
