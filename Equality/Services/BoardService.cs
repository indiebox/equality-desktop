using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class BoardService : BoardServiceBase<Board>, IBoardService
    {
        public BoardService(IApiClient apiClient, ITokenResolver tokenResolver) : base(apiClient, tokenResolver)
        {

        }
    }
}