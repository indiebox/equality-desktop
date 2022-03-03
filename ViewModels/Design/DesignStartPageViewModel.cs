using Catel.Collections;

using Equality.Models;

namespace Equality.ViewModels.Design
{
    public class DesignStartPageViewModel : StartPageViewModel
    {
        public DesignStartPageViewModel() : base(null)
        {
            Invites.AddRange(new Invite[]
            {
                new Invite()
                {
                    Team = new Team() { Name = "Long team name Long team name" },
                    Inviter = new User() { Name = "Long user name Long user name" },
                },
                new Invite()
                {
                    Team = new Team() { Name = "Test team" },
                    Inviter = new User() { Name = "User 1" },
                },
                new Invite()
                {
                    Team = new Team() { Name = "Test team 2" },
                    Inviter = new User() { Name = "User 2" },
                },
                new Invite()
                {
                    Team = new Team() { Name = "Test team 2" },
                    Inviter = new User() { Name = "User 2" },
                },
            });
        }
    }
}
