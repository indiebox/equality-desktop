using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Data;
using Equality.Models;

namespace Equality.Services
{
    public interface IUserServiceBase<TUserModel>
        where TUserModel : class, IUser, new()
    {
        /// <summary>
        /// Sends the request to get an authenticated user to the API.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TUserModel>> LoadAuthUserAsync(QueryParameters query = null);

        /// <summary>
        /// Sends the login request to the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The password email.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TUserModel>> LoginAsync(string email, string password);

        /// <summary>
        /// Sends the logout authenticated user request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// <para>Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.</para>
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
}
