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

namespace Equality.Data
{
    public class ExceptionWatcher
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        private readonly IExceptionService _exceptionService;

        private readonly INotificationService _notificationService;

        public ExceptionWatcher(IExceptionService exceptionService, INotificationService notificationService)
        {
            Argument.IsNotNull(nameof(exceptionService), exceptionService);
            Argument.IsNotNull(nameof(notificationService), notificationService);

            _exceptionService = exceptionService;
            _notificationService = notificationService;

            RegisterHandlers();
            SubscribeEvents();
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
