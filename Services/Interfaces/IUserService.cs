using System;
using System.Threading.Tasks;

using Equality.Core.ApiClient;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Sends the request to get an authenticated user to the API.
        /// </summary>
        /// <param name="token">The api token.</param>
        /// <returns>Returns the authenticated <see cref="User"/>.</returns>
        /// <exception cref="ArgumentException" />
        public Task<User> GetUserAsync(string token);

        /// <summary>
        /// Sends the login request to the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The password email.</param>
        /// <returns>Returns the authenticated <see cref="User"/> and api token.</returns>
        public Task<(User user, string token)> LoginAsync(string email, string password);

        /// <summary>
        /// Sends the logout authenticated user request to the API.
        /// </summary>
        /// <param name="token">The api token.</param>
        /// <returns>Returns the API response.</returns>
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> LogoutAsync(string token);

        /// <summary>
        /// Sends the forgot password request to the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>Returns the server response as a <see cref="JObject"/>.</returns>
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
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> ResetPasswordAsync(string email, string password, string passwordConfirmation, string token);

        /// <summary>
        /// Deserializes the JSON string to the <see cref="User"/> model.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized <see cref="User"/> model.</returns>
        /// <exception cref="ArgumentException" />
        public User Deserialize(string data);
    }
}
