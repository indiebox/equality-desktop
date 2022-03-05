using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Data;
using Equality.Models;

namespace Equality.Services
{
    public interface IUserService : IApiDeserializable<IUser>
    {
        /// <summary>
        /// Sends the request to get an authenticated user to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets token from <see cref="IStateManager.ApiToken"></see>.
        /// <code></code>
        /// After success response sets <see cref="IStateManager.CurrentUser"></see>.
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
        /// <remarks>
        /// After success login sets <see cref="IStateManager.ApiToken"></see> and <see cref="IStateManager.CurrentUser"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> LoginAsync(string email, string password);

        /// <summary>
        /// Sends the logout authenticated user request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets the token from <see cref="IStateManager.ApiToken"></see>.
        /// <code></code>
        /// After success logout sets <c><see cref="IStateManager.ApiToken"></see> = null</c> and <c><see cref="IStateManager.CurrentUser"></see> = null</c>.
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
}
