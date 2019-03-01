using Autofac;
using Axity.Security.Core.Interfaces.UseCases;
using Axity.Security.Core.UseCases;

namespace Axity.Security.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RegisterUserUseCase>().As<IRegisterUserUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<LoginUseCase>().As<ILoginUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<ExchangeRefreshTokenUseCase>().As<IExchangeRefreshTokenUseCase>().InstancePerLifetimeScope();
            builder.RegisterType<AddRoleUserUseCase>().As<IAddRoleUserUseCase>().InstancePerLifetimeScope();
        }
    }
}
