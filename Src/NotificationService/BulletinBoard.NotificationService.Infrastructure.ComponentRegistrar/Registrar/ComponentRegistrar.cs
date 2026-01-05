using BulletinBoard.Infrastructure.DataAccess.Contexts.User.EmailSender;
using BulletinBoard.NotificationService.AppServices.Common;
using BulletinBoard.NotificationService.AppServices.Common.Behaviors.LoggingBehavior;
using BulletinBoard.NotificationService.AppServices.Common.IRepository;
using BulletinBoard.NotificationService.AppServices.Notification.Mail;
using BulletinBoard.NotificationService.Infrastructure.Repository;
using BulletinBoard.NotificationService.Infrastructure.Repository.CRepository.BaseRepository;
using BulletinBoard.NotificationService.Infrastructure.Repository.QRepository.BaseRepository;
using BulletinBoard.NotificationService.AppServices.Common.Behaviors.TransactionBehavior;
using BulletinBoard.NotificationService.AppServices.Common.Behaviors.ValidatingBehavior;
using FluentValidation;
using MassTransit;
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


        return services;
    }

    private static IServiceCollection RegistrarInfComponents(this IServiceCollection services)
    {
        services.AddScoped(typeof(ICommandRepository<>), typeof(CommandRepository<>));
        services.AddScoped(typeof(IQueryRepository<>), typeof(QueryRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IEmailSender, YandexSmtpEmailSender>();

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
