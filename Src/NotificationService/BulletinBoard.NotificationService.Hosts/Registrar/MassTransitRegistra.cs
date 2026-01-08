using BulletinBoard.NotificationService.Hosts.EventBusConsumer.User;
using MassTransit;

namespace BulletinBoard.NotificationService.Hosts.Registrar;

public static class MassTransitRegistra
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserAddedConsumer>();
            x.AddConsumer<UserEmailConfirmationStartedEventConsumer>();
            x.AddConsumer<UserResetPasswordStartedEventConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", host =>
                {
                    host.Username("rmuser");
                    host.Password("rmpassword");
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}