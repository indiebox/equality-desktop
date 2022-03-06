using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;

namespace Equality.Core.Services
{
    public class UserService : IUserService
    {
        protected IApiClient ApiClient;

        protected ITokenResolverService TokenResolver;

        public UserService(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public async Task<ApiResponseMessage> LoadAuthUserAsync()
            => await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync("user");

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

            return await ApiClient.PostAsync("login", data);
        }

        public async Task<ApiResponseMessage> LogoutAsync()
            => await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync("logout");

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
    }
}
