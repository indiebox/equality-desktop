using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Catel.IoC;
using Catel.Services;

using Equality.Data;
using Equality.Http;
using Equality.MVVM;
using Equality.Services;

using PusherClient;

namespace Equality.ViewModels
{
    public class LoadingWindowViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected IUserService UserService;

        public LoadingWindowViewModel(IUIVisualizerService uiVisualizerService, IUserService userService)
        {
            UIVisualizerService = uiVisualizerService;
            UserService = userService;
        }

        public override string Title => "Equality";

        protected async Task HandleAuthenticatedUser()
        {
            string apiToken = Properties.Settings.Default.api_token;

            try {
                bool result = await IsValidToken(apiToken);

                if (result) {
                    RegisterPusher();
                    OpenMainPage();
                } else {
                    OpenAuthorizationPage();
                }

                await CloseViewModelAsync(true);
            } catch (Exception e) {
                ExceptionHandler.Handle(e);
            }
        }

        protected async Task<bool> IsValidToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token)) {
                return false;
            }

            StateManager.ApiToken = token;

            try {
                var response = await UserService.LoadAuthUserAsync();
                StateManager.CurrentUser = response.Object;

                return true;
            } catch (UnauthorizedHttpException) {
                StateManager.ApiToken = null;

                Properties.Settings.Default.api_token = "";
                Properties.Settings.Default.Save();

                return false;
            } catch {
                StateManager.ApiToken = null;

                throw;
            }
        }

        protected void RegisterPusher()
        {
            var client = new Http.PusherClient("7c6a91460be1e040ce8c", new PusherOptions
            {
                Authorizer = new HttpAuthorizer("http://equality/broadcasting/auth")
                {
                    AuthenticationHeader = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", StateManager.ApiToken)
                },
                Cluster = "eu",
                Encrypted = true,
            });

            void HandleError(object sender, PusherException error)
            {
                if ((int)error.PusherCode < 5000) {
                    // Error recevied from Pusher cluster, use PusherCode to filter.
                } else {
                    if (error is ChannelUnauthorizedException unauthorizedAccess) {
                        // Private and Presence channel failed authorization with Forbidden (403)
                    } else if (error is ChannelAuthorizationFailureException httpError) {
                        // Authorization endpoint returned an HTTP error other than Forbidden (403)
                    } else if (error is OperationTimeoutException timeoutError) {
                        // A client operation has timed-out. Governed by PusherOptions.ClientTimeout
                    } else if (error is ChannelDecryptionException decryptionError) {
                        // Failed to decrypt the data for a private encrypted channel
                    } else {
                        // Handle other errors
                    }
                }

                Trace.TraceError($"{error}");
            }
            client.Error += HandleError;

            ServiceLocator.Default.RegisterInstance<IWebsocketClient>(client);
        }

        protected void OpenMainPage()
        {
            UIVisualizerService.ShowAsync<ApplicationWindowViewModel>();
        }

        protected void OpenAuthorizationPage()
        {
            UIVisualizerService.ShowAsync<AuthorizationWindowViewModel>();
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await HandleAuthenticatedUser();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
