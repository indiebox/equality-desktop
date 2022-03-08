using System.Runtime.CompilerServices;

using Catel.IoC;
using Catel.Services;

using Equality.Services;

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
        | Overriding default types
        |--------------------------------------------------------------------------
        |
        | Here we overriding default types for Dependency Injection.
        |
        */

        // We need to set RegistrationType.Transient to INavigationService and INavigationRootService,
        // so we can work with multiple windows with frame at the same time (previously not supported).
        // It will also allow NavigationService to continue working even after reopening the same window (previously not supported).
        // Also we register custom INavigationRootService, so we can navigate in the specified context by the NavigationServiceExtension.
        serviceLocator.RegisterType<INavigationService, NavigationService>(RegistrationType.Transient);
        serviceLocator.RegisterType<INavigationRootService, Equality.MVVM.NavigationRootService>(RegistrationType.Transient);

        /*
        |--------------------------------------------------------------------------
        | Register types
        |--------------------------------------------------------------------------
        |
        | Here we register types in the ServiceLocator for Dependency Injection.
        |
        */

        serviceLocator.RegisterType<IUserService, UserService>();
        serviceLocator.RegisterType<ITeamService, TeamService>();
        serviceLocator.RegisterType<IInviteService, InviteService>();
        serviceLocator.RegisterType<IProjectService, ProjectService>();
    }
}