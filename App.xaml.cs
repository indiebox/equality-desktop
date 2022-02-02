using System.Windows;

using Catel.IoC;
using Catel.Logging;

using Equality.Core.ApiClient;
using Equality.Core.ApiClient.Interfaces;
using Equality.Models;

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

            // Log.Info("Improving performance");
            // Catel.Windows.Controls.UserControl.DefaultCreateWarningAndErrorValidatorForViewModelValue = false;
            // Catel.Windows.Controls.UserControl.DefaultSkipSearchingForInfoBarMessageControlValue = true;

            /*
            |--------------------------------------------------------------------------
            | Register types
            |--------------------------------------------------------------------------
            |
            | Here we register custom types in the ServiceLocator for Dependency Injection.
            | 
            | For singleton registration we should use serviceLocator.RegisterInstance<>(instance);
            |
            */

            Log.Info("Registering custom types");

            var serviceLocator = ServiceLocator.Default;

            var apiClient = new ApiClient();
            serviceLocator.RegisterInstance<IApiClient>(apiClient);

            // To auto-forward styles, check out Orchestra (see https://github.com/wildgums/orchestra)
            // StyleHelper.CreateStyleForwardersForDefaultStyles();

            Log.Info("Calling base.OnStartup");

            base.OnStartup(e);
        }
    }
}