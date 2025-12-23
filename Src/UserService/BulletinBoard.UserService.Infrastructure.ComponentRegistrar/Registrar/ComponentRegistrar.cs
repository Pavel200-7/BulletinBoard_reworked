using BulletinBoard.UserService.AppServices.Common;
using BulletinBoard.UserService.AppServices.Common.Behaviors.LoggingBehavior;
using BulletinBoard.UserService.AppServices.Common.Behaviors.TransactionBehavior;
using BulletinBoard.UserService.AppServices.Common.Behaviors.ValidatingBehavior;
using BulletinBoard.UserService.AppServices.Common.UnitOfWork;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.UserService.AppServices.User.Queries.Helpers.RefreshToken;
using BulletinBoard.UserService.AppServices.User.Repositiry;
using BulletinBoard.UserService.Infrastructure.DataAccess.Common.UnitOfWork;
using BulletinBoard.UserService.Infrastructure.Repository;
using BulletinBoard.UserService.Infrastructure.Repository.IRepository;
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

        services.RegistrarBLLComponents();
        services.RegistrarDALComponents();

        services.RegistrarBehaviors();
        return services;
    }

    private static IServiceCollection RegistrarBLLComponents(this IServiceCollection services)
    {
        services.AddScoped<IJWTProvider, JWTProvider>();
        services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();


        return services;
    }

    private static IServiceCollection RegistrarDALComponents(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

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
