using System;
using System.Collections.Generic;
using System.Text;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class BoardService : BoardServiceBase<Board, Project>, IBoardService
    {
        public BoardService(IApiClient apiClient, ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {

        }
    }
}