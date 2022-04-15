using System;
using System.Threading.Tasks;

using Equality.Models;

namespace Equality.Services
{
    public partial interface IProjectServiceBase<TProjectModel, TTeamModel, TLeaderNominationModel, TUserModel>
        where TProjectModel : class, IProject, new()
        where TTeamModel : class, ITeam, new()
        where TLeaderNominationModel : class, ILeaderNomination, new()
        where TUserModel : class, IUser, new()
    {
        /// <summary>
        /// Subscribe to websocket event for nominate user.
        /// </summary>
        /// <param name="project">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the array of leader nominations.
        /// </param>
        public Task SubscribeNominateUserAsync(IProject project, Action<TLeaderNominationModel[]> action);

        /// <summary>
        /// Unsubscribe from websocket event for nominate user.
        /// </summary>
        /// <param name="project">The project.</param>
        public void UnsubscribeNominateUser(IProject project);
    }
}
