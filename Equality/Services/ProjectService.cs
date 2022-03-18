﻿using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class ProjectService : ProjectServiceBase<Project, Team, LeaderNomination>, IProjectService
    {
        public ProjectService(IApiClient apiClient, ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {

        }
    }
}