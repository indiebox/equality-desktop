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

        public Task<JObject> ResetPasswordAsync(string email);

        /// <summary>
        /// Deserializes the JSON string to the <see cref="User"/> model.
        /// </summary>
        /// <param name="data">The JSON string.</param>
        /// <returns>The deserialized <see cref="User"/> model.</returns>
        public User Deserialize(string data);
    }
}
