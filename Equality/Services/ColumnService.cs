using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    internal class ColumnService : ColumnServiceBase<Column>, IColumnService
    {
        public ColumnService(IApiClient apiClient, ITokenResolver tokenResolver, IWebsocketClient websocketClient)
            : base(apiClient, tokenResolver, websocketClient)
        {
        }
    }
}
