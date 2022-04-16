using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface ICardServiceBase<TCardModel> : IDeserializeModels<TCardModel>
        where TCardModel : class, ICard, new()
    {
        /// <summary>
        /// Sends the get cards request to the API.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TCardModel[]>> GetCardsAsync(IColumn column, QueryParameters query = null);

        /// <inheritdoc cref="GetCardsAsync(IColumn, QueryParameters)"/>
        /// <param name="columnId">The column id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TCardModel[]>> GetCardsAsync(ulong columnId, QueryParameters query = null);

        /// <summary>
        /// Sends the create card for the column request to the API.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="card">The card.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(IColumn column, ICard card, QueryParameters query = null);

        /// <inheritdoc cref="CreateCardAsync(IColumn, ICard, QueryParameters)"/>
        /// <param name="columnId">The column id.</param>
        /// <param name="card">The card.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, ICard card, QueryParameters query = null);

        /// <summary>
        /// Sends the create card for the column after specified card request to the API.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <param name="card">The card.</param>
        /// <param name="afterCardId">
        /// The id of the card after which you need to create a new one.
        /// If '0' - creates new card at first position.
        /// </param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, ICard card, ulong? afterCardId, QueryParameters query = null);

        /// <summary>
        /// Sends the update card request to the API.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TCardModel>> UpdateCardAsync(ICard card, QueryParameters query = null);

        /// <summary>
        /// Sends the update card order request to the API.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="afterCard">The card after which insert new. If null - insert card at first position.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> UpdateCardOrderAsync(ICard card, ICard afterCard);

        /// <inheritdoc cref="UpdateCardOrderAsync(ICard, ICard)" />
        /// <param name="cardId">The card id.</param>
        /// <param name="afterCardId">The card id after which insert new. If 0 - insert card at first position.</param>
        public Task<ApiResponseMessage> UpdateCardOrderAsync(ulong cardId, ulong afterCardId);

        /// <summary>
        /// Sends the delete card request to the API.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeleteCardAsync(ICard card);

        /// <inheritdoc cref="DeleteCardAsync(ICard)" />
        /// <param name="cardId">The card id.</param>
        public Task<ApiResponseMessage> DeleteCardAsync(ulong cardId);
    }
}
