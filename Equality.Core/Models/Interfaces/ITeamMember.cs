using System;

namespace Equality.Models
{
    public interface ITeamMember : IUser
    {
        public DateTime JoinedAt { get; set; }

        public bool IsCreator { get; set; }
    }
}
