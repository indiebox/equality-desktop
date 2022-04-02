using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public class CardService : CardServiceBase<Card, Column>, ICardService
    {
        public CardService(IApiClient apiClient, ITokenResolverService tokenResolver) : base(apiClient, tokenResolver)
        {
        }
    }
}