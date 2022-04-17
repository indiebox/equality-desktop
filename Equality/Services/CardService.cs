using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class CardService : CardServiceBase<Card>, ICardService
    {
        public CardService(IApiClient apiClient, ITokenResolver tokenResolver, IWebsocketClient websocketClient)
            : base(apiClient, tokenResolver, websocketClient)
        {
        }
    }
}