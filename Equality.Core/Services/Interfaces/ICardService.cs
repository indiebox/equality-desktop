using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;

namespace Equality.Services
{
    public interface ICardServiceBase<TCardModel, TColumnModel> : IDeserializeModels<TCardModel>
        where TCardModel : class, ICard, new()
        where TColumnModel : class, IColumn, new()
    {
        /// <summary>
        /// Sends the get cards request to the API.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TCardModel[]>> GetCardsAsync(TColumnModel column, QueryParameters query = null);

        /// <inheritdoc cref="GetCardsAsync(TColumnModel, QueryParameters)"/>
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
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(TColumnModel column, TCardModel card, QueryParameters query = null);

        /// <inheritdoc cref="CreateCardAsync(TColumnModel, TCardModel, QueryParameters)"/>
        /// <param name="columnId">The column id.</param>
        /// <param name="card">The card.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, TCardModel card, QueryParameters query = null);

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
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TCardModel>> CreateCardAsync(ulong columnId, TCardModel card, ulong? afterCardId, QueryParameters query = null);

        /// <summary>
        /// Sends the delete card request to the API.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeleteCardAsync(TCardModel card);

        /// <inheritdoc cref="DeleteCardAsync(TCardModel)" />
        /// <param name="cardId">The card id.</param>
        public Task<ApiResponseMessage> DeleteCardAsync(ulong cardId);
    }
}
