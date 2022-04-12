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
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
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
        /// Gets a <c>token</c> using <see cref="ITokenResolver.ResolveApiToken"></see> and <c>socket-id</c> using <see cref="ITokenResolver.ResolveSocketID"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(TBoardModel board, TColumnModel column, QueryParameters query = null);

        /// <inheritdoc cref="CreateColumnAsync(TBoardModel, TColumnModel, QueryParameters)"/>
        /// <param name="boardId">The board id.</param>
        /// <param name="column">The column.</param>
        /// <param name="query">The query parameters.</param>
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(ulong boardId, TColumnModel column, QueryParameters query = null);

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
        public Task<ApiResponseMessage<TColumnModel>> UpdateColumnAsync(TColumnModel column, QueryParameters query = null);

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
        /// Gets a token using <see cref="ITokenResolver.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage> DeleteColumnAsync(TColumnModel column);

        /// <inheritdoc cref="DeleteColumnAsync(TColumnModel)" />
        /// <param name="columnId">The column id.</param>
        public Task<ApiResponseMessage> DeleteColumnAsync(ulong columnId);

        #region Websockets

        /// <summary>
        /// Subscribe to websocket event for create column.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the created column.
        /// Second is the id of column after which the created column should be inserted.
        /// If second argument is null - last position. If 0 - first position.
        /// </param>
        public Task SubscribeCreateColumnAsync(IBoard board, Action<TColumnModel, ulong?> action);

        /// <summary>
        /// Unsubscribe from websocket event for create column.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeCreateColumn(IBoard board);

        /// <summary>
        /// Subscribe to websocket event for update column.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the updated column.
        /// </param>
        public Task SubscribeUpdateColumnAsync(IBoard board, Action<TColumnModel> action);

        /// <summary>
        /// Unsubscribe from websocket event for update column.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeUpdateColumn(IBoard board);

        /// <summary>
        /// Subscribe to websocket event for update column order.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the id of column.
        /// Second is the id of column after which the column should be moved.
        /// If second argument is 0 - first position.
        /// </param>
        public Task SubscribeUpdateColumnOrderAsync(IBoard board, Action<ulong, ulong> action);

        /// <summary>
        /// Unsubscribe from websocket event for update column order.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeUpdateColumnOrder(IBoard board);

        /// <summary>
        /// Subscribe to websocket event for delete column.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the id of deleted column.
        /// </param>
        public Task SubscribeDeleteColumnAsync(IBoard board, Action<ulong> action);

        /// <summary>
        /// Unsubscribe from websocket event for delete column.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeDeleteColumn(IBoard board);

        #endregion
    }
}
