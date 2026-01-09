using BulletinBoard.NotificationService.Infrastructure.Services.OAuth.GitHubOAuth.Helpers.GitHubHttp;
using BulletinBoard.UserService.AppServices.Common;
using BulletinBoard.UserService.AppServices.Common.Behaviors.Logging;
using BulletinBoard.UserService.AppServices.Common.Behaviors.Transaction;
using BulletinBoard.UserService.AppServices.Common.Behaviors.Validating;
using BulletinBoard.UserService.AppServices.Common.IRepository;
using BulletinBoard.UserService.AppServices.User.Helpers.Repositiry;
using BulletinBoard.UserService.AppServices.User.User.Commands.OAuthRegister.Helpers.OAuth.Factories;
using BulletinBoard.UserService.AppServices.User.User.Helpers.JWT;
using BulletinBoard.UserService.AppServices.User.User.Helpers.RefreshT;
using BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor;
using BulletinBoard.UserService.AppServices.User.User.Queries.GetOAuthProviderURI.Helpers.URIConstructor.Factories;
using BulletinBoard.UserService.Infrastructure.Repository;
using BulletinBoard.UserService.Infrastructure.Repository.CRepository.BaseRepository;
using BulletinBoard.UserService.Infrastructure.Repository.QRepository;
using BulletinBoard.UserService.Infrastructure.Repository.QRepository.BaseRepository;
using BulletinBoard.UserService.Infrastructure.Services.OAuth.Factories;
using BulletinBoard.UserService.Infrastructure.Services.OAuth.GitHubOAuth;
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
