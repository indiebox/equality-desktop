using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Core.ApiClient;
using Equality.Core.StateManager;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class UserService : IUserService
    {
        protected IApiClient ApiClient;

        protected IStateManager StateManager;

        public UserService(IApiClient apiClient, IStateManager stateManager)
        {
            ApiClient = apiClient;
            StateManager = stateManager;
        }

        public async Task<ApiResponseMessage> LoadAuthUserAsync()
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).GetAsync("user");

            StateManager.CurrentUser = Deserialize(response.Content["data"]);

            return response;
        }

        public async Task<ApiResponseMessage> LoginAsync(string email, string password)
        {
            Argument.IsNotNullOrWhitespace(nameof(email), email);
            Argument.IsNotNullOrEmpty(nameof(password), password);

            Dictionary<string, object> data = new()
            {
                { "email", email },
                { "password", password },
                { "device_name", GetDeviceName() },
            };

            var response = await ApiClient.PostAsync("login", data);

            var user = Deserialize(response.Content["data"]);
            string token = response.Content["token"].ToString();

            StateManager.ApiToken = token;
            StateManager.CurrentUser = user;

            return response;
        }

        public async Task<ApiResponseMessage> LogoutAsync()
        {
            Argument.IsNotNullOrWhitespace("IStateManager.ApiToken", StateManager.ApiToken);

            var response = await ApiClient.WithTokenOnce(StateManager.ApiToken).PostAsync("logout");

            StateManager.ApiToken = null;
            StateManager.CurrentUser = null;

            return response;
        }

        public async Task<ApiResponseMessage> SendResetPasswordTokenAsync(string email)
        {
            Argument.IsNotNullOrWhitespace(nameof(email), email);

            Dictionary<string, object> data = new()
            {
                { "email", email },
            };

            return await ApiClient.PostAsync("forgot-password", data);
        }

        public async Task<ApiResponseMessage> ResetPasswordAsync(string email, string password, string passwordConfirmation, string token)
        {
            Argument.IsNotNullOrWhitespace(nameof(email), email);
            Argument.IsNotNullOrEmpty(nameof(password), password);
            Argument.IsNotNullOrEmpty(nameof(passwordConfirmation), passwordConfirmation);
            Argument.IsNotNullOrWhitespace(nameof(token), token);

            Dictionary<string, object> data = new()
            {
                { "email", email },
                { "password", password },
                { "password_confirmation", passwordConfirmation },
                { "token", token },
            };

            return await ApiClient.PostAsync("reset-password", data);
        }

        protected string GetDeviceName()
        {
            return Environment.MachineName;
        }

        /// <inheritdoc cref="IApiDeserializable{T}.Deserialize(JToken)"/>
        protected User Deserialize(JToken data) => ((IUserService)this).Deserialize(data);

        /// <inheritdoc cref="IApiDeserializable{T}.DeserializeRange(JToken)"/>
        protected User[] DeserializeRange(JToken data) => ((IUserService)this).DeserializeRange(data);
    }
}
