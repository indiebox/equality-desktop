using System.Threading.Tasks;

using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Sends the login request to the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The password email.</param>
        /// <returns>Returns the authenticated <see cref="User"/> and api token.</returns>
        public Task<(User user, string token)> LoginAsync(string email, string password);

        /// <summary>
        /// Sends a request to send a message with a password recovery code to the mail on the API.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <returns>Returns the server response as a <see cref="JObject"/>.</returns>
        public Task<JObject> ForgotPasswordEmailSendAsync(string email);

        /// <summary>
        /// Sends a request to the API to reset the password.
        /// </summary>
        /// <param name="email">The user email.</param>
        /// <param name="password">The user password.</param>
        /// <param name="passwordConfirmation">The user re-entered password.</param>
        /// <param name="token">A token from a letter sent to an email.</param>
        /// <returns>Returns the server response as a <see cref="JObject"/>.</returns>
        public Task<JObject> ResetPasswordAsync(string email, string password, string passwordConfirmation, string token);

        /// <summary>
        /// Deserializes the JSON string to the <see cref="User"/> model.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized <see cref="User"/> model.</returns>
        public User Deserialize(string data);
    }
}
