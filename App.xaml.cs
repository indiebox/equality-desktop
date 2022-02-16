﻿using System.Windows;

using Catel.IoC;
using Catel.Logging;
using Catel.MVVM;
using Catel.Services;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Services;

namespace Equality
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogManager.AddDebugListener();
#endif

            Log.Info("Starting application");

            // Want to improve performance? Uncomment the lines below. Note though that this will disable
            // some features. 
            //
            // For more information, see http://docs.catelproject.com/vnext/faq/performance-considerations/

            Log.Info("Improving performance");
            Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;


            /*
            |--------------------------------------------------------------------------
            | Overriding default types
            |--------------------------------------------------------------------------
            |
            | Here we overriding default types for Dependency Injection.
            |
            */

            Log.Info("Overriding default types");

            var serviceLocator = ServiceLocator.Default;

            // We need to set RegistrationType.Transient to INavigationService and INavigationRootService,
            // so we can work with multiple windows with frame at the same time (previously not supported).
            // It will also allow NavigationService to continue working even after reopening the same window (previously not supported).
            serviceLocator.RegisterType<INavigationService, NavigationService>(RegistrationType.Transient);
            serviceLocator.RegisterType<INavigationRootService, NavigationRootService>(RegistrationType.Transient);

            /*
            |--------------------------------------------------------------------------
            | Register types
            |--------------------------------------------------------------------------
            |
            | Here we register custom types in the ServiceLocator for Dependency Injection.
            |
            */

            Log.Info("Registering custom types");

            serviceLocator.RegisterType<IApiClient, ApiClient>();
            serviceLocator.RegisterType<IStateManager, StateManager>();

            serviceLocator.RegisterType<IUserService, UserService>();
            serviceLocator.RegisterType<ITeamService, TeamService>();

            /*
            |--------------------------------------------------------------------------
            | Add custom naming conventions
            |--------------------------------------------------------------------------
            |
            | Here we add all folders inside Views/ and ViewModels/ to naming conventions,
            | so they will be resolved by Catel correctly.
            |
            | You can register naming conventions in 3 different places:
            | * IUrlLocator (for INavigationService)
            | * IViewLocator (for IUIVisualizerService)
            | * IViewModelLocator (to search for a VM for a View)
            |
            | In fact, IViewLocator and IViewModelLocator use namespace, 
            | while IUrlLocator uses path. Therefore, if you split ViewModels and Views into subfolders, 
            | but their namespace remained untouched ([Assembly].ViewModels or [Assembly].Views), 
            | then you don't need to add your own rules to IViewLocator and IViewModelLocator.
            | 
            | If you are not using INavigationService, you actually dont need add custom naming conventions to
            | IUrlLocator.
            */

            Log.Info("Registering custom naming conventions");

            var urlLocator = serviceLocator.ResolveType<IUrlLocator>();
            urlLocator.NamingConventions.Add("/Views/Authorization/[VM].xaml");
            urlLocator.NamingConventions.Add("/Views/Projects/[VM].xaml");
            urlLocator.NamingConventions.Add("/Views/Teams/[VM].xaml");

            //var viewLocator = ServiceLocator.Default.ResolveType<IViewLocator>();
            //viewLocator.NamingConventions.Add("[UP].Views.Authorization.[VM]");

            //var viewModelLocator = ServiceLocator.Default.ResolveType<IViewModelLocator>();
            //viewModelLocator.NamingConventions.Add("[UP].ViewModels.Authorization.[VM]ViewModel");

            // To auto-forward styles, check out Orchestra (see https://github.com/wildgums/orchestra)
            // StyleHelper.CreateStyleForwardersForDefaultStyles();

            Log.Info("Calling base.OnStartup");

            base.OnStartup(e);
        }
    }
}