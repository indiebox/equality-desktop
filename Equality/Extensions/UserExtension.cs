using Equality.Data;
using Equality.Models;

namespace Equality.Extensions
{
    public static class UserExtension
    {
        public static bool IsCurrentUser(this User @this)
        {
            return @this == StateManager.CurrentUser;
        }
    }
}
