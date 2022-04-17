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
    }
}
