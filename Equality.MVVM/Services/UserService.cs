using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class UserService : Core.Services.UserService, IUserService
    {
        public UserService(IApiClient apiClient, Core.Services.ITokenResolverService stateManager) : base(apiClient, stateManager)
        {
        }

        public new async Task<ApiResponseMessage<User>> LoadAuthUserAsync()
        {
            var response = await base.LoadAuthUserAsync();

            var user = Deserialize(response.Content["data"]);

            return new(user, response);
        }

        public new async Task<ApiResponseMessage<User>> LoginAsync(string email, string password)
        {
            var response = await base.LoginAsync(email, password);

            var user = Deserialize(response.Content["data"]);

            return new(user, response);
        }

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.Deserialize(JToken)"/>
        protected User Deserialize(JToken data) => ((IUserService)this).Deserialize(data);

        /// <inheritdoc cref="Core.Services.IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected User[] DeserializeRange(JToken data) => ((IUserService)this).DeserializeRange(data);
    }
}
