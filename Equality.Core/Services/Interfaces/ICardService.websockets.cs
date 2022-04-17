using System;
using System.Threading.Tasks;

using Equality.Models;

namespace Equality.Services
{
    public partial interface ICardServiceBase<TCardModel>
        where TCardModel : class, ICard, new()
    {
        /// <summary>
        /// Subscribe to websocket event for create card.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the created card.
        /// Second is the id of the column.
        /// Third is the id of card after which the created card should be inserted.
        /// If third argument is null - last position. If 0 - first position.
        /// </param>
        public Task SubscribeCreateCardAsync(IBoard board, Action<TCardModel, ulong, ulong?> action);

        /// <summary>
        /// Unsubscribe from websocket event for create card.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeCreateCard(IBoard board);

        /// <summary>
        /// Subscribe to websocket event for update card.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the updated card.
        /// </param>
        public Task SubscribeUpdateCardAsync(IBoard board, Action<TCardModel> action);

        /// <summary>
        /// Unsubscribe from websocket event for update card.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeUpdateCard(IBoard board);

        /// <summary>
        /// Subscribe to websocket event for update card order.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the id of card.
        /// Second is the id of card after which the card should be moved.
        /// If second argument is 0 - first position.
        /// </param>
        public Task SubscribeUpdateCardOrderAsync(IBoard board, Action<ulong, ulong> action);

        /// <summary>
        /// Unsubscribe from websocket event for update card order.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeUpdateCardOrder(IBoard board);

        /// <summary>
        /// Subscribe to websocket event for move card to column.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the id of moved card.
        /// Second is the id of column.
        /// Third is the id of card after which the created card should be inserted.
        /// If third argument is null - last position. If 0 - first position.
        /// </param>
        public Task SubscribeMoveCardToColumnAsync(IBoard board, Action<ulong, ulong, ulong> action);

        /// <summary>
        /// Unsubscribe from websocket event for move card to column.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeMoveCardToColumn(IBoard board);

        /// <summary>
        /// Subscribe to websocket event for delete card.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="action">
        /// The action.
        /// First argument is the id of deleted card.
        /// </param>
        public Task SubscribeDeleteCardAsync(IBoard board, Action<ulong> action);

        /// <summary>
        /// Unsubscribe from websocket event for delete card.
        /// </summary>
        /// <param name="board">The board.</param>
        public void UnsubscribeDeleteCard(IBoard board);
    }
}
