using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;


namespace Equality.Services
{
    public interface IColumnService<TColumnModel, TBoardModel> : IDeserializeModels<TColumnModel>
        where TColumnModel : class, IColumn, new()
        where TBoardModel : class, IBoard, new()
    {
        /// <summary>
        /// Sends the get board columns request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(TBoardModel board, QueryParameters query = null);

        /// <inheritdoc cref="GetColumnsAsync(TBoardModel, QueryParameters)"/>
        /// <param name="boardId">The board id.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(ulong boardId, QueryParameters query = null);

        /// <summary>
        /// Sends the create column request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="column">The column.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(TBoardModel board, TColumnModel column, QueryParameters query = null);

        /// <inheritdoc cref="CreateColumnAsync(TBoardModel, TColumnModel, QueryParameters)"/>
        /// <param name="boardId">The board id.</param>
        /// <param name="column">The column.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(ulong boardId, TColumnModel column, QueryParameters query = null);

        /// Sends the update column request to the API.
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
        public Task<ApiResponseMessage<TColumnModel>> UpdateColumnAsync(TColumnModel column, QueryParameters query = null);

        /// <summary>
        /// Sends the update column order request to the API.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="afterColumn">The column after which insert new. If null - insert column at first position.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> UpdateColumnOrderAsync(TColumnModel column, TColumnModel afterColumn);

        /// <inheritdoc cref="UpdateColumnOrderAsync(TColumnModel, TColumnModel)" />
        /// <param name="columnId">The column id.</param>
        /// <param name="afterColumnId">The column id after which insert new. If 0 - insert column at first position.</param>
        public Task<ApiResponseMessage> UpdateColumnOrderAsync(ulong columnId, ulong afterColumnId);

        /// <summary>
        /// Sends the delete column request to the API.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeleteColumnAsync(TColumnModel column);

        /// <inheritdoc cref="DeleteColumnAsync(TColumnModel)" />
        /// <param name="columnId">The column id.</param>
        public Task<ApiResponseMessage> DeleteColumnAsync(ulong columnId);
    }
}
