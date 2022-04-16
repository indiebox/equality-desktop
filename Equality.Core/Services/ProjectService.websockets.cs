using System;
using System.Threading.Tasks;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public partial class ProjectServiceBase<TProjectModel, TLeaderNominationModel, TUserModel>
        where TProjectModel : class, IProject, new()
        where TLeaderNominationModel : class, ILeaderNomination, new()
        where TUserModel : class, IUser, new()
    {
        protected IWebsocketClient WebsocketClient;

        public ProjectServiceBase(IApiClient apiClient, ITokenResolver tokenResolver, IWebsocketClient websocketClient)
            : this(apiClient, tokenResolver)
        {
            WebsocketClient = websocketClient;
        }

        public async Task SubscribeNominateUserAsync(IProject project, Action<TLeaderNominationModel[]> action)
        {
            await WebsocketClient.BindEventAsync(GetChannelName(project), "leader-nominated", (data) =>
            {
                var deserializedNominations = Json.Deserialize<TLeaderNominationModel[]>(data["nominations"].ToString());

                action.Invoke(deserializedNominations);
            });
        }

        public void UnsubscribeNominateUser(IProject project)
        {
            WebsocketClient.UnbindEvent(GetChannelName(project), "leader-nominated");
        }

        protected string GetChannelName(IProject project) => $"private-projects.{project.Id}";
    }
}
