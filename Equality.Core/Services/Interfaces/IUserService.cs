using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Data;

namespace Equality.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Sends the request to get an authenticated user to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> LoadAuthUserAsync();

        /// <summary>
        /// Sends the login request to the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The password email.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> LoginAsync(string email, string password);

        /// <summary>
        /// Sends the logout authenticated user request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// <para>Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.</para>
        /// After this request this token will be invalid.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> LogoutAsync();

        /// <summary>
        /// Sends the forgot password request to the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> SendResetPasswordTokenAsync(string email);

        /// <summary>
        /// Sends the reset password request to the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The user password.</param>
        /// <param name="passwordConfirmation">The user re-entered password.</param>
        /// <param name="token">The token from an email.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> ResetPasswordAsync(string email, string password, string passwordConfirmation, string token);
    }

    public interface IUserService<TUserModel> : IDeserializeModels<TUserModel>
        where TUserModel : class, new()
    {
        /// <inheritdoc cref="IUserService.LoadAuthUserAsync" />
        public Task<ApiResponseMessage<TUserModel>> LoadAuthUserAsync();

        /// <inheritdoc cref="IUserService.LoginAsync(string, string)" />
        public Task<ApiResponseMessage<TUserModel>> LoginAsync(string email, string password);

        /// <inheritdoc cref="IUserService.LogoutAsync" />
        public Task<ApiResponseMessage> LogoutAsync();

        /// <inheritdoc cref="IUserService.SendResetPasswordTokenAsync(string)" />
        public Task<ApiResponseMessage> SendResetPasswordTokenAsync(string email);

        /// <inheritdoc cref="IUserService.ResetPasswordAsync(string, string, string, string)" />
        public Task<ApiResponseMessage> ResetPasswordAsync(string email, string password, string passwordConfirmation, string token);
    }
}
