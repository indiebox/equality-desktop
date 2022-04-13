using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Threading;

using Catel;
using Catel.ExceptionHandling;
using Catel.IoC;
using Catel.Logging;
using Catel.Services;

using Equality.Converters;
using Equality.Helpers;
using Equality.Http;
using Equality.Services;
using Equality.ViewModels;

using PusherClient;

namespace Equality.Data
{
    public class ExceptionHandler
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private static readonly IExceptionService _exceptionService;

        private readonly INotificationService _notificationService;

        static ExceptionHandler()
        {
            _exceptionService = ServiceLocator.Default.ResolveType<IExceptionService>();
        }

        public ExceptionHandler(INotificationService notificationService)
        {
            Argument.IsNotNull(nameof(notificationService), notificationService);

            _notificationService = notificationService;

            RegisterHandlers();
            SubscribeEvents();
        }

        public static IExceptionService Service => _exceptionService;

        /// <summary>
        /// Handle the exception.
        /// </summary>
        /// <param name="e">The exception.</param>
        /// <returns>Returns <see langword="true"/> if exception is handler, <see langword="false"/> otherwise.</returns>
        public static bool Handle(Exception e)
        {
            Log.Error(e);

            return _exceptionService.HandleException(e);
        }

        private void RegisterHandlers()
        {
            _exceptionService.Register<UnprocessableEntityHttpException>(e =>
            {
                throw e;
            });

            _exceptionService.Register<BadRequestHttpException>(HandleBadRequestException);
            _exceptionService.Register<UnauthorizedHttpException>(HandleUnauthorizedException);
            _exceptionService.Register<ForbiddenHttpException>(HandleForbiddenException);
            _exceptionService.Register<NotFoundHttpException>(HandleNotFoundException);
            _exceptionService.Register<TooManyRequestsHttpException>(HandleTooManyRequestsException);
            _exceptionService.Register<ServerErrorHttpException>(HandleServerErrorException);
            _exceptionService.Register<HttpRequestException>(HandleHttpRequestException);

            _exceptionService.Register<PusherException>(HandlePusherException)
                .OnErrorRetry(3, TimeSpan.FromSeconds(1));
        }

        #region HttpExceptions

        private void HandleBadRequestException(BadRequestHttpException exception)
        {
            Log.Error(exception.Message);

            _notificationService.ShowError("Ошибка запроса. Пожалуйста, свяжитесь с разработчиками.");
        }

        #region Unauthorized

        private void HandleUnauthorizedException(UnauthorizedHttpException exception)
        {
            StateManager.ApiToken = null;
            StateManager.CurrentUser = null;

            Properties.Settings.Default.api_token = "";
            Properties.Settings.Default.Save();

            var vm = MvvmHelper.CreateViewModel<AuthorizationWindowViewModel>();
            vm.InitializedAsync += AuthorizationWindowVmInitializedAsync;
            ServiceLocator.Default.ResolveType<IUIVisualizerService>().ShowAsync(vm);
        }

        private Task AuthorizationWindowVmInitializedAsync(object sender, EventArgs e)
        {
            var vm = (AuthorizationWindowViewModel)sender;
            vm.InitializedAsync -= AuthorizationWindowVmInitializedAsync;

            _notificationService.ShowError("Ошибка авторизации.\nПожалуйста, выполните повторый вход.");

            return Task.CompletedTask;
        }

        #endregion

        private void HandleForbiddenException(ForbiddenHttpException exception)
        {
            _notificationService.ShowError("Ошибка доступа.", TimeSpan.FromSeconds(5));
        }

        private void HandleNotFoundException(NotFoundHttpException exception)
        {
            if (exception.Message == string.Empty) {
                _notificationService.ShowError($"Запрашиваемой страницы не существует ({exception.Url}).");
            } else {
                _notificationService.ShowError("Запись не найдена.", TimeSpan.FromSeconds(5));
            }
        }

        private void HandleTooManyRequestsException(TooManyRequestsHttpException exception)
        {
            var converter = new PluralizeConverter()
            {
                One = "секунда",
                Two = "секунды",
                Five = "секунд",
            };
            var seconds = (string)converter.Convert(exception.RetryAfter.Value.Seconds, null, null, System.Globalization.CultureInfo.CurrentCulture);
            _notificationService.ShowError($"Достигнут лимит запросов. Повторите через {seconds}.", TimeSpan.FromSeconds(5));
        }

        private void HandleServerErrorException(ServerErrorHttpException exception)
        {
            _notificationService.ShowError("Ошибка сервера.\nПожалуйста, попробуйте позже.");
        }

        private void HandleHttpRequestException(HttpRequestException exception)
        {
            _notificationService.ShowError($"Ошибка соединения:\n{exception.Message}");
        }

        #endregion

        #region PusherExceptions

        private void HandlePusherException(PusherException exception)
        {
            Log.Warning(exception);

            _notificationService.ShowWarning($"Ошибка подключения к Pusher: {exception.Message}");
        }

        #endregion

        #region Subscribe to events

        private void SubscribeEvents()
        {
            var appDomain = AppDomain.CurrentDomain;
            appDomain.UnhandledException += OnAppDomainUnhandledException;

            var dispatcher = App.Current.Dispatcher;
            if (dispatcher != null) {
                dispatcher.UnhandledException += OnDispatcherUnhandledException;
            }

            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
        }

        private void OnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            var exception = args.Exception;
            if (exception != null) {
                Log.Error(exception, "TaskScheduler.UnobservedTaskException occurred");

                if (_exceptionService.HandleException(exception.InnerException)) {
                    args.SetObserved();
                }
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
