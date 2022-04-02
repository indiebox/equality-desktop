using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

using Newtonsoft.Json.Linq;

namespace Equality.Services
{
    public class CardServiceBase<TCardModel, TColumnModel> : ICardServiceBase<TCardModel, TColumnModel>
        where TCardModel : class, ICard, new()
        where TColumnModel : class, IColumn, new()
    {
        protected ITokenResolverService TokenResolver;

        protected IApiClient ApiClient;

        public CardServiceBase(IApiClient apiClient, ITokenResolverService tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }
        public Task<ApiResponseMessage<TCardModel[]>> GetCardsAsync(TColumnModel column, QueryParameters query = null)
            => GetCardsAsync(column.Id, query);

        public async Task<ApiResponseMessage<TCardModel[]>> GetCardsAsync(ulong columnId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(columnId), columnId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"columns/{columnId}/cards"));

            var boards = DeserializeRange(response.Content["data"]);

            return new(boards, response);
        }

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TCardModel Deserialize(JToken data) => ((IDeserializeModels<TCardModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TCardModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TCardModel>)this).DeserializeRange(data);
    }
}
