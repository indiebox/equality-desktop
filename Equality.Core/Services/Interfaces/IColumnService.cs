using System;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;
using Equality.Data;


namespace Equality.Services
{
    public partial interface IColumnService<TColumnModel> : IDeserializeModels<TColumnModel>
        where TColumnModel : class, IColumn, new()
    {
        /// <summary>
        /// Sends the get board columns request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(IBoard board, QueryParameters query = null);

        /// <inheritdoc cref="GetColumnsAsync(IBoard, QueryParameters)"/>
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
        /// Gets a <c>token</c> using <see cref="ITokenResolver.ResolveApiToken"></see> and <c>socket-id</c> using <see cref="ITokenResolver.ResolveSocketID"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(IBoard board, IColumn column, QueryParameters query = null);

        /// <inheritdoc cref="CreateColumnAsync(IBoard, IColumn, QueryParameters)"/>
        /// <param name="boardId">The board id.</param>
        /// <param name="column">The column.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(ulong boardId, IColumn column, QueryParameters query = null);

        /// <summary>
        /// Sends the update column request to the API.
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
        public Task<ApiResponseMessage<TColumnModel>> UpdateColumnAsync(IColumn column, QueryParameters query = null);

        /// <summary>
        /// Sends the update column order request to the API.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="afterColumn">The column after which insert new. If null - insert column at first position.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> UpdateColumnOrderAsync(IColumn column, IColumn afterColumn);

        /// <inheritdoc cref="UpdateColumnOrderAsync(IColumn, IColumn)" />
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
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeleteColumnAsync(IColumn column);

        /// <inheritdoc cref="DeleteColumnAsync(IColumn)" />
        /// <param name="columnId">The column id.</param>
        public Task<ApiResponseMessage> DeleteColumnAsync(ulong columnId);
    }
}
