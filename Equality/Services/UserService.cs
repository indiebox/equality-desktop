using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class UserService : UserServiceBase<User>, IUserService
    {
        public UserService(IApiClient apiClient, ITokenResolver stateManager) : base(apiClient, stateManager)
        {
        }
    }
}
