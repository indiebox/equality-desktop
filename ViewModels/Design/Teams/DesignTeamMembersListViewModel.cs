using System;

using Catel.Collections;

using Equality.Models;

namespace Equality.ViewModels.Design
{
    public class DesignTeamMembersListViewModel : TeamMembersListViewModel
    {
        public DesignTeamMembersListViewModel() : base(null)
        {
            FilteredMembers.AddRange(new TeamMember[] {
                new TeamMember() { Name = "user1", JoinedAt = DateTime.Now.AddHours(-2) },
                new TeamMember() { Name = "user2", JoinedAt = DateTime.Now.AddDays(-1).AddHours(-1) },
                new TeamMember() { Name = "user3", JoinedAt = DateTime.Now.AddDays(-1).AddHours(-4) },
                new TeamMember() { Name = "Пользователь 4", JoinedAt = DateTime.Now.AddDays(-2).AddHours(-5) },
                new TeamMember() { Name = "Пользователь 5", JoinedAt = DateTime.Now.AddDays(-3).AddHours(-5) },
                new TeamMember() { Id = 1, Name = "Текущий пользователь", JoinedAt = DateTime.Now.AddDays(-3).AddHours(-5) },
            });
        }
    }
}
