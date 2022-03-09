using System.Runtime.CompilerServices;

using Catel.IoC;

using Equality.Core.Services;
using Equality.Http;

/// <summary>
/// All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        // Code added here will be executed as soon as the assembly is loaded by the .net runtime. This
        // is a great opportunity to register any services in the service locator:

        var serviceLocator = ServiceLocator.Default;

        /*
        |--------------------------------------------------------------------------
        | Register types
        |--------------------------------------------------------------------------
        |
        | Here we register types in the ServiceLocator for Dependency Injection.
        |
        */

        serviceLocator.RegisterType<IApiClient, ApiClient>();

        serviceLocator.RegisterType<IUserService, UserService>();
        serviceLocator.RegisterType<ITeamService, TeamService>();
        serviceLocator.RegisterType<IProjectService, ProjectService>();
    }
}