using MassTransit;

namespace BulletinBoard.UserService.Hosts.Registrar;

public static class MassTransitRegistra
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
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