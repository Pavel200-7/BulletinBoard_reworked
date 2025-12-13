using BulletinBoard.UserService.AppServices.Auth.Repositories.IAuthServiceAdapter;
using BulletinBoard.UserService.AppServices.Common.AssembliesNovigation;
using BulletinBoard.UserService.AppServices.Study;
using BulletinBoard.UserService.AppServices.Study.Helpers;
using BulletinBoard.UserService.Infrastructure.Identity;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace BulletinBoard.Infrastructure.ComponentRegistrar.Registrar;

public static class ComponentRegistgar
{
    public static IServiceCollection RegistrarComponents(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(AssembliesNovigationClass).Assembly);
        services.AddScoped<ISomeHelper, SomeHelper>();
        services.AddScoped<IStudyService, StudyService>();


        return services;
    }


    public static IServiceCollection RegistrarInfrastructureComponents(this IServiceCollection services)
    {
        services.AddScoped<IAuthServiceAdapter, AuthServiceAdapter>();   

        return services;
    }


}
