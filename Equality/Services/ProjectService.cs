using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class ProjectService : ProjectServiceBase<Project, Team, LeaderNomination, User>, IProjectService
    {
        public ProjectService(IApiClient apiClient, ITokenResolver tokenResolver, IWebsocketClient websocketClient)
            : base(apiClient, tokenResolver, websocketClient)
        {
        }
    }
}
