using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using Catel.Reflection;

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

        // var serviceLocator = ServiceLocator.Default;

        /*
        |--------------------------------------------------------------------------
        | Assemblies management
        |--------------------------------------------------------------------------
        |
        | excludeTypeCacheAssemblies - the build data will be ignored in Catel's TypeCacheEvaluator.
        | This will prevent some program freezes while clearing the cache of these assemblies.
        | 
        | preloadAssemblies - Assemblies that should be preloaded before the application start.
        |
        */
        string[] excludeTypeCacheAssemblies = new string[] {
            "Windows.UI",
        };
        TypeCacheEvaluator.AssemblyEvaluators.Add((assembly) => excludeTypeCacheAssemblies.Contains<string>(assembly.GetName().Name));

        string[] preloadAssemblies = new string[] {
            "System.Runtime.WindowsRuntime",
            "Accessibility",
        };
        foreach (string assembly in preloadAssemblies) {
            try {
                Assembly.Load(assembly);
            } catch { }
        }
    }
}