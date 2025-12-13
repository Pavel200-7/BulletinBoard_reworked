using ESourcerGenerator.Registrar;
using Microsoft.Extensions.DependencyInjection;

namespace BulletinBoard.UserService.Infrastructure.ComponentRegistrar.Registrar;

public static class DecoratorsRegistrar
{
    public static IServiceCollection RegistrarMethodLogDecorators(this IServiceCollection services)
    {
        services.RegistrarDecorators(MethodLogDecoratorRegistrar.GetAllDecoratorsType());
        return services;
    }

    private static IServiceCollection RegistrarDecorators(this IServiceCollection services, IEnumerable<Type> decoratorsType)
    {
        foreach (var decoratorType in decoratorsType)
        {
            var interfaceType = decoratorType.GetInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                Console.WriteLine($"{decoratorType.FullName} зарегистрирован.");
                services.Decorate(interfaceType, decoratorType);
            }
        }
        return services;
    }
}
