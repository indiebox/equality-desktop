using Equality.Models;

namespace Equality.Data
{
    public interface IStateManager
    {
        /// <summary>
        /// Gets or sets the current authenticated user.
        /// </summary>
        public User CurrentUser { get; set; }

        /// <summary>
        /// Gets or sets the currently used Api token.
        /// </summary>
        public string ApiToken { get; set; }
    }
}
