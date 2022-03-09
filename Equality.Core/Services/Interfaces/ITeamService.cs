using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Core.Services
{
    public interface ITeamService
    {
        /// <summary>
        /// Sends the get current user teams request to the API.
        /// </summary>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> GetTeamsAsync();

        /// <summary>
        /// Sends the create team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> CreateAsync(ITeam team);

        /// <summary>
        /// Sends the get members request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> GetMembersAsync(ITeam team);

        /// <inheritdoc cref="GetMembersAsync(ITeam)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage> GetMembersAsync(ulong teamId);

        /// <summary>
        /// Sends the leave team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> LeaveTeamAsync(ITeam team);

        /// <inheritdoc cref="LeaveTeamAsync(ITeam)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage> LeaveTeamAsync(ulong teamId);

        /// <summary>
        /// Sends the set logo request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <param name="imagePath">The path to image file.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> SetLogoAsync(ITeam team, string imagePath);

        /// <inheritdoc cref="SetLogoAsync(ITeam, string)"/>
        /// <param name="teamId">The team id.</param>
        /// <param name="imagePath">The path to image file.</param>
        public Task<ApiResponseMessage> SetLogoAsync(ulong teamId, string imagePath);

        /// <summary>
        /// Sends the delete logo request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeleteLogoAsync(ITeam team);

        /// <inheritdoc cref="DeleteLogoAsync(ITeam)"/>
        /// <param name="teamId">The team id.</param>
        public Task<ApiResponseMessage> DeleteLogoAsync(ulong teamId);

        /// <summary>
        /// Sends the update team request to the API.
        /// </summary>
        /// <param name="team">The team.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> UpdateTeamAsync(ITeam team);
    }

    public interface ITeamService<TTeamModel, TTeamMemberModel> : IDeserializeModels<TTeamModel>
        where TTeamModel : class, new()
        where TTeamMemberModel : class, new()
    {
        /// <inheritdoc cref="ITeamService.GetTeamsAsync" />
        public Task<ApiResponseMessage<TTeamModel[]>> GetTeamsAsync();

        /// <inheritdoc cref="ITeamService.CreateAsync(ITeam)" />
        public Task<ApiResponseMessage<TTeamModel>> CreateAsync(TTeamModel team);

        /// <inheritdoc cref="ITeamService.GetMembersAsync(ITeam)" />
        public Task<ApiResponseMessage<TTeamMemberModel[]>> GetMembersAsync(TTeamModel team);

        /// <inheritdoc cref="ITeamService.GetMembersAsync(ulong)" />
        public Task<ApiResponseMessage<TTeamMemberModel[]>> GetMembersAsync(ulong teamId);

        /// <inheritdoc cref="ITeamService.LeaveTeamAsync(ITeam)" />
        public Task<ApiResponseMessage> LeaveTeamAsync(TTeamModel team);

        /// <inheritdoc cref="ITeamService.LeaveTeamAsync(ulong)" />
        public Task<ApiResponseMessage> LeaveTeamAsync(ulong teamId);

        /// <inheritdoc cref="ITeamService.SetLogoAsync(ITeam, string)" />
        public Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(TTeamModel team, string imagePath);

        /// <inheritdoc cref="ITeamService.SetLogoAsync(ulong, string)" />
        public Task<ApiResponseMessage<TTeamModel>> SetLogoAsync(ulong teamId, string imagePath);

        /// <inheritdoc cref="ITeamService.DeleteLogoAsync(ITeam)" />
        public Task<ApiResponseMessage<TTeamModel>> DeleteLogoAsync(TTeamModel team);

        /// <inheritdoc cref="ITeamService.DeleteLogoAsync(ulong)" />
        public Task<ApiResponseMessage<TTeamModel>> DeleteLogoAsync(ulong teamId);

        /// <inheritdoc cref="ITeamService.UpdateTeamAsync(ITeam)" />
        public Task<ApiResponseMessage<TTeamModel>> UpdateTeamAsync(TTeamModel team);
    }
}
