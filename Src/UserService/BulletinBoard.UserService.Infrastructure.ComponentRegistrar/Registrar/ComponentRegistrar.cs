using BulletinBoard.UserService.AppServices.Common;
using BulletinBoard.UserService.AppServices.Common.Behaviors.LoggingBehavior;
using BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;
using BulletinBoard.UserService.AppServices.Common.Behaviors.ValidatingBehavior;
using BulletinBoard.UserService.AppServices.Common.UnitOfWork;
using BulletinBoard.UserService.Infrastructure.DataAccess.Common.UnitOfWork;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;

public static class ComponentRegistrar
{
    public static IServiceCollection RegistrarComponents(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssembliesNavigationAppServices).Assembly));
        services.AddValidatorsFromAssembly(typeof(AssembliesNavigationAppServices).Assembly);
        services.AddAutoMapper(typeof(AssembliesNavigationAppServices).Assembly);

        // Все остальные зависимости
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.RegistrarBehaviors();
        return services;
    }

    private static IServiceCollection RegistrarBehaviors(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatingBehavior<,>));
        return services;
    }
}
