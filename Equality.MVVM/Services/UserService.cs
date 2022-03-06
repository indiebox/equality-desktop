using System.Threading.Tasks;

using Equality.Http;
using Equality.Data;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class UserService : Core.Services.UserService, IUserService
    {
        public UserService(IApiClient apiClient, IStateManager stateManager) : base(apiClient, stateManager)
        {
        }

        public override async Task<ApiResponseMessage> LoadAuthUserAsync()
        {
            var response = await base.LoadAuthUserAsync();

            StateManager.CurrentUser = Deserialize(response.Content["data"]);

            return response;
        }

        public override async Task<ApiResponseMessage> LoginAsync(string email, string password)
        {
            var response = await base.LoginAsync(email, password);

            var user = Deserialize(response.Content["data"]);
            StateManager.CurrentUser = user;

            return response;
        }

        public override async Task<ApiResponseMessage> LogoutAsync()
        {
            var response = await base.LogoutAsync();

            StateManager.CurrentUser = null;

            return response;
        }

        /// <inheritdoc cref="Core.Services.IApiDeserializable{T}.Deserialize(JToken)"/>
        protected User Deserialize(JToken data) => ((IUserService)this).Deserialize(data);

        /// <inheritdoc cref="Core.Services.IApiDeserializable{T}.DeserializeRange(JToken)"/>
        protected User[] DeserializeRange(JToken data) => ((IUserService)this).DeserializeRange(data);
    }
}
