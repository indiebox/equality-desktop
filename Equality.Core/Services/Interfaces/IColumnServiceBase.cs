using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Equality.Http;
using Equality.Models;

namespace Equality.Services
{
    public interface IColumnServiceBase<TColumnModel, TBoardModel> : IDeserializeModels<TColumnModel>
        where TColumnModel : class, IColumn, new()
        where TBoardModel : class, IBoard, new()
    {
        /// <summary>
        /// Sends the get board columns request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(IBoard board);

        /// <inheritdoc cref="GetColumnsAsync(ITeam)"/>
        /// <param name="boardId">The board id.</param>
        public Task<ApiResponseMessage<TColumnModel[]>> GetColumnsAsync(ulong boardId);

        /// <summary>
        /// Sends the create column request to the API.
        /// </summary>
        /// <param name="board">The board.</param>
        /// <param name="column">The column.</param>
        /// <returns>Returns the API response.</returns>
        /// 
        /// <remarks>
        /// Gets a token using <see cref="ITokenResolverService.ResolveApiToken"></see>.
        /// </remarks>
        /// 
        /// <exception cref="ArgumentException" />
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(TBoardModel board, TColumnModel column);

        /// <inheritdoc cref="CreateBoardAsync(TBoardModel, TColumnModel)"/>
        /// <param name="boardId">The project id.</param>
        public Task<ApiResponseMessage<TColumnModel>> CreateColumnAsync(ulong boardId, TColumnModel column);
    }
}
