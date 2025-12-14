using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;
using BulletinBoard.UserService.AppServices.Common.AssembliesNavigation;
using BulletinBoard.UserService.AppServices.Common.Behaviors.LoggingBehavior;
using BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;
using BulletinBoard.UserService.AppServices.Common.Behaviors.ValidatingBehavior;
using BulletinBoard.UserService.AppServices.Common.UnitOfWork;
using BulletinBoard.UserService.Infrastructure.DataAccess.Common.AssembliesNavigation;
using BulletinBoard.UserService.Infrastructure.DataAccess.Common.UnitOfWork;
using BulletinBoard.UserService.Infrastructure.Identity;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;

public static class ComponentRegistgar
{
    public static IServiceCollection RegistrarComponents(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(AssembliesNavigationAppServices).Assembly,
            typeof(AssembliesNavigationInfrastructureDA).Assembly);

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssembliesNavigationAppServices).Assembly));
        services.AddValidatorsFromAssembly(typeof(AssembliesNavigationAppServices).Assembly);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatingBehavior<,>));


        return services;
    }


    public static IServiceCollection RegistrarInfrastructureComponents(this IServiceCollection services)
    {
        services.AddScoped<IAuthServiceAdapter, AuthServiceAdapter>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
