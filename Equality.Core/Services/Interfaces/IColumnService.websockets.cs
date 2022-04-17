using System;
using System.Threading.Tasks;

using Equality.Models;

namespace Equality.Services
{
    public partial interface IColumnService<TColumnModel>
        where TColumnModel : class, IColumn, new()
    {
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
    }
}
