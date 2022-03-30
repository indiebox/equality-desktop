using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.IoC;
using Catel.Services;

using Equality.Helpers;
using Equality.Http;
using Equality.Services;
using Equality.ViewModels;

namespace Equality.Data
{
    public static class HttpExceptionHandler
    {
        static INotificationService NotificationService;

        static HttpExceptionHandler()
        {
            NotificationService = ServiceLocator.Default.ResolveType<INotificationService>();
        }

        public static bool HandleException(HttpRequestException exception)
        {
            var result = exception switch
            {
                UnprocessableEntityHttpException => false,
                UnauthorizedHttpException ex => HandleUnauthorizedException(ex),
                ForbiddenHttpException ex => HandleForbiddenException(ex),
                NotFoundHttpException ex => HandleNotFoundException(ex),
                TooManyRequestsHttpException ex => HandleTooManyRequestsException(ex),
                _ => HandleHttpRequestException(exception),
            };

            if (result) {
                Debug.WriteLine(exception.ToString());
            }

            return result;
        }

        #region Unauthorized

        public static bool HandleUnauthorizedException(UnauthorizedHttpException exception)
        {
            StateManager.ApiToken = null;
            StateManager.CurrentUser = null;

            Properties.Settings.Default.api_token = "";
            Properties.Settings.Default.Save();

            var vm = MvvmHelper.CreateViewModel<AuthorizationWindowViewModel>();
            vm.InitializedAsync += AuthorizationWindowVmInitializedAsync;
            ServiceLocator.Default.ResolveType<IUIVisualizerService>().ShowAsync(vm);

            return true;
        }

        private static Task AuthorizationWindowVmInitializedAsync(object sender, EventArgs e)
        {
            var vm = (AuthorizationWindowViewModel)sender;
            vm.InitializedAsync -= AuthorizationWindowVmInitializedAsync;

            NotificationService.ShowError("Ошибка авторизации.\nПожалуйста, выполните повторый вход.");

            return Task.CompletedTask;
        }

        #endregion

        public static bool HandleForbiddenException(ForbiddenHttpException exception) => throw new NotImplementedException();

        public static bool HandleNotFoundException(NotFoundHttpException exception) => throw new NotImplementedException();

        public static bool HandleTooManyRequestsException(TooManyRequestsHttpException exception) => throw new NotImplementedException();

        public static bool HandleHttpRequestException(HttpRequestException exception) => throw new NotImplementedException();

        //public void HandleServerErrorException()
    }
}
