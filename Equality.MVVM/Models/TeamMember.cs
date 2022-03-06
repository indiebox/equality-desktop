using System;

namespace Equality.Models
{
    public class TeamMember : User
    {
        public DateTime JoinedAt { get; set; }

        public bool IsCreator { get; set; }
    }
}
