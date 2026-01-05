using BulletinBoard.NotificationService.AppServices.Common;
using BulletinBoard.NotificationService.AppServices.Common.Behaviors.LoggingBehavior;
using BulletinBoard.NotificationService.AppServices.Common.Behaviors.Transaction;
using BulletinBoard.NotificationService.AppServices.Common.Behaviors.ValidatingBehavior;
using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using BulletinBoard.NotificationService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth;
using BulletinBoard.NotificationService.AppServices.User.Commands.OAuthRegister.Helpers.OAuth.Factories;
using BulletinBoard.NotificationService.AppServices.User.Queries.Helpers.JWTGenerator;
using BulletinBoard.NotificationService.AppServices.User.Queries.Helpers.RefreshT;
using BulletinBoard.NotificationService.AppServices.User.Repositiry;
using BulletinBoard.NotificationService.Infrastructure.Repository;
using BulletinBoard.NotificationService.Infrastructure.Repository.CRepository.BaseRepository;
using BulletinBoard.NotificationService.Infrastructure.Repository.QRepository;
using BulletinBoard.NotificationService.Infrastructure.Repository.QRepository.BaseRepository;
using BulletinBoard.NotificationService.Infrastructure.Services.OAuth.Factories;
using BulletinBoard.NotificationService.Infrastructure.Services.OAuth.GitHubOAuth;
using BulletinBoard.NotificationService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp;
using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;
using BulletinBoard.UserService.AppServices.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor.Factories;
using FluentValidation;
using MassTransit;
using MassTransit.MultiBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.NotificationService.Infrastructure.ComponentRegistrar.Registrar;

public static class ComponentRegistrar
{
    public static IServiceCollection RegistrarComponents(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssembliesNavigationAppServices).Assembly));
        services.AddValidatorsFromAssembly(typeof(AssembliesNavigationAppServices).Assembly);
        services.AddAutoMapper(typeof(AssembliesNavigationAppServices).Assembly);

        services.RegistrarBLLComponents();
        services.RegistrarInfComponents();

        services.RegistrarBehaviors();
        return services;
    }

    private static IServiceCollection RegistrarBLLComponents(this IServiceCollection services)
    {
        services.AddScoped<IJWTProvider, JWTProvider>();
        services.AddScoped<IRefreshTokenProvider, RefreshTokenProvider>();

        services.AddScoped<GitHubURIConstructor>();
        services.AddScoped<IURIConstructorFactory, URIConstructorFactory>();


        services.AddScoped<GitHubOAuthService>();

        return services;
    }

    private static IServiceCollection RegistrarInfComponents(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        services.AddScoped<GitHubOAuthService>();
        services.AddScoped<IOAuthServiceFactory, OAuthServiceFactory>();

        services.AddScoped<IGitHubHttpService, GitHubHttpService>();

        //services.AddScoped<IOAuthService, OAuthService>();

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
