using System.Collections.Generic;
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

        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(TColumnModel column, TCardModel card, QueryParameters query = null)
            => CreateCardAsync(column.Id, card, query);

        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, TCardModel card, QueryParameters query = null)
            => CreateCardAsync(columnId, card, null, query);

        public async Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, TCardModel card, ulong? afterCardId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(columnId), columnId, 1);
            Argument.IsNotNull(nameof(card), card);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", card.Name },
                { "description", card.Description },
            };

            if (afterCardId != null) {
                data.Add("after_card", afterCardId);
            }

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync(query.Parse($"columns/{columnId}/cards"), data);

            card = Deserialize(response.Content["data"]);

            return new(card, response);
        }

        public async Task<ApiResponseMessage<TCardModel>> UpdateCardAsync(TCardModel card, QueryParameters query = null)
        {
            Argument.IsNotNull(nameof(card), card);
            Argument.IsMinimal<ulong>("card.Id", card.Id, 1);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", card.Name },
                { "description", card.Description },
            };

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PatchAsync(query.Parse($"cards/{card.Id}"), data);

            card = Deserialize(response.Content["data"]);

            return new(card, response);
        }

        public Task<ApiResponseMessage> UpdateCardOrderAsync(TCardModel card, TCardModel afterCard)
            => UpdateCardOrderAsync(card.Id, afterCard?.Id ?? 0);

        public async Task<ApiResponseMessage> UpdateCardOrderAsync(ulong cardId, ulong afterCardId)
        {
            Argument.IsMinimal<ulong>(nameof(cardId), cardId, 1);

            Dictionary<string, object> data = new()
            {
                { "after", afterCardId.ToString() },
            };

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"cards/{cardId}/order", data);
        }

        public Task<ApiResponseMessage> DeleteCardAsync(TCardModel card)
            => DeleteCardAsync(card.Id);

        public async Task<ApiResponseMessage> DeleteCardAsync(ulong cardId)
        {
            Argument.IsMinimal<ulong>(nameof(cardId), cardId, 1);

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).DeleteAsync($"cards/{cardId}");
        }

        /// <inheritdoc cref="IDeserializeModels{T}.Deserialize(JToken)"/>
        protected TCardModel Deserialize(JToken data) => ((IDeserializeModels<TCardModel>)this).Deserialize(data);

        /// <inheritdoc cref="IDeserializeModels{T}.DeserializeRange(JToken)"/>
        protected TCardModel[] DeserializeRange(JToken data) => ((IDeserializeModels<TCardModel>)this).DeserializeRange(data);
    }
}
