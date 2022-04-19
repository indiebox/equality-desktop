using System.Collections.Generic;
using System.Threading.Tasks;

using Catel;

using Equality.Data;
using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public partial class CardServiceBase<TCardModel> : ICardServiceBase<TCardModel>
        where TCardModel : class, ICard, new()
    {
        protected ITokenResolver TokenResolver;

        protected IApiClient ApiClient;

        public CardServiceBase(IApiClient apiClient, ITokenResolver tokenResolver)
        {
            Argument.IsNotNull(nameof(apiClient), apiClient);
            Argument.IsNotNull(nameof(tokenResolver), tokenResolver);

            ApiClient = apiClient;
            TokenResolver = tokenResolver;
        }

        public Task<ApiResponseMessage<TCardModel[]>> GetCardsAsync(IColumn column, QueryParameters query = null)
            => GetCardsAsync(column.Id, query);

        public async Task<ApiResponseMessage<TCardModel[]>> GetCardsAsync(ulong columnId, QueryParameters query = null)
        {
            Argument.IsMinimal<ulong>(nameof(columnId), columnId, 1);
            query ??= new QueryParameters();

            var response = await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).GetAsync(query.Parse($"columns/{columnId}/cards"));

            var boards = Json.Deserialize<TCardModel[]>(response.Content["data"]);

            return new(boards, response);
        }

        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(IColumn column, ICard card, QueryParameters query = null)
            => CreateCardAsync(column.Id, card, query);

        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, ICard card, QueryParameters query = null)
            => CreateCardAsync(columnId, card, null, query);

        public async Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, ICard card, ulong? afterCardId, QueryParameters query = null)
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

            var response = await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .PostAsync(query.Parse($"columns/{columnId}/cards"), data);
            var deserializedCard = Json.Deserialize<TCardModel>(response.Content["data"]);

            return new(deserializedCard, response);
        }

        public async Task<ApiResponseMessage<TCardModel>> UpdateCardAsync(ICard card, QueryParameters query = null)
        {
            Argument.IsNotNull(nameof(card), card);
            Argument.IsMinimal<ulong>("card.Id", card.Id, 1);
            query ??= new QueryParameters();

            Dictionary<string, object> data = new()
            {
                { "name", card.Name },
                { "description", card.Description },
            };

            var response = await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .PatchAsync(query.Parse($"cards/{card.Id}"), data);
            var deserializedCard = Json.Deserialize<TCardModel>(response.Content["data"]);

            return new(deserializedCard, response);
        }

        public Task<ApiResponseMessage> UpdateCardOrderAsync(ICard card, ICard afterCard)
            => UpdateCardOrderAsync(card.Id, afterCard?.Id ?? 0);

        public async Task<ApiResponseMessage> UpdateCardOrderAsync(ulong cardId, ulong afterCardId)
        {
            Argument.IsMinimal<ulong>(nameof(cardId), cardId, 1);

            Dictionary<string, object> data = new()
            {
                { "after", afterCardId.ToString() },
            };

            return await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .PostAsync($"cards/{cardId}/order", data);
        }

        public Task<ApiResponseMessage> MoveCardToColumnAsync(ICard card, IColumn column, ICard afterCard)
            => MoveCardToColumnAsync(card.Id, column.Id, afterCard?.Id ?? 0);

        public async Task<ApiResponseMessage> MoveCardToColumnAsync(ulong cardId, ulong columnId, ulong afterCardId)
        {
            Argument.IsMinimal<ulong>(nameof(cardId), cardId, 1);
            Argument.IsMinimal<ulong>(nameof(columnId), columnId, 1);

            Dictionary<string, object> data = new()
            {
                { "after_card", afterCardId.ToString() },
            };

            return await ApiClient.WithTokenOnce(TokenResolver.ResolveApiToken()).PostAsync($"cards/{cardId}/move/{columnId}", data);
        }

        public Task<ApiResponseMessage> DeleteCardAsync(ICard card)
            => DeleteCardAsync(card.Id);

        public async Task<ApiResponseMessage> DeleteCardAsync(ulong cardId)
        {
            Argument.IsMinimal<ulong>(nameof(cardId), cardId, 1);

            return await ApiClient
                .WithTokenOnce(TokenResolver.ResolveApiToken())
                .WithSocketID(TokenResolver.ResolveSocketID())
                .DeleteAsync($"cards/{cardId}");
        }
    }
}
