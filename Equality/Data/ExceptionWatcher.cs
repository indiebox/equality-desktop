using System;
using System.Net.Http;
using System.Windows.Threading;

using Catel;
using Catel.ExceptionHandling;
using Catel.Logging;

namespace Equality.Data
{
    public class ExceptionWatcher
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IExceptionService _exceptionService;

        public ExceptionWatcher(IExceptionService exceptionService)
        {
            Argument.IsNotNull(() => exceptionService);

            _exceptionService = exceptionService;

            RegisterHandlers();
            SubscribeEvents();
        }

        private void RegisterHandlers()
        {
            _exceptionService.Register<HttpRequestException>(async exception =>
            {
                if (!HttpExceptionHandler.HandleException(exception)) {
                    throw exception;
                }
            });
        }

        #region Subscribe to events

        private void SubscribeEvents()
        {
            var appDomain = AppDomain.CurrentDomain;
            appDomain.UnhandledException += OnAppDomainUnhandledException;

            var dispatcher = App.Current.Dispatcher;
            if (dispatcher != null) {
                dispatcher.UnhandledException += OnDispatcherUnhandledException;
            }
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = args.ExceptionObject as Exception;
            if (exception != null) {
                Log.Error(exception, "AppDomain.UnhandledException occurred");

                _exceptionService.HandleException(exception);
            }
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var exception = args.Exception;
            if (exception != null) {
                Log.Error(exception, "Dispatcher.UnhandledException occurred");

                if (_exceptionService.HandleException(exception)) {
                    args.Handled = true;
                }
            }
        }

        #endregion
    }
}
