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
    }
}
