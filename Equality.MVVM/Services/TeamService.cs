﻿using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class TeamService : TeamServiceBase<Team, TeamMember>, ITeamService
    {
        public TeamService(IApiClient apiClient, ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {
        }
    }
}
