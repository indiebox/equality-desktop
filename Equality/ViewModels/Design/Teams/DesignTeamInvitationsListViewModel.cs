using System;

using Catel.Collections;

using Equality.Models;

namespace Equality.ViewModels.Design
{
    public class DesignTeamInvitationsListViewModel : TeamInvitationsListViewModel
    {
        public DesignTeamInvitationsListViewModel() : base(null)
        {
            FilteredInvites.AddRange(new Invite[]
            {
                new Invite()
                {
                    Inviter = new User()
                    {
                        Name = "IgorGaming"
                    },
                    Invited = new User()
                    {
                        Name = "RedMarshall"
                    },
                    CreatedAt = DateTime.Now,
                    Status = IInvite.InviteStatus.Pending,
                },
                new Invite()
                {
                    Invited = new User()
                    {
                        Name = "Borya"
                    },
                    AcceptedAt = DateTime.Now.AddMinutes(-3),
                    Status = IInvite.InviteStatus.Accepted,
                },
                new Invite()
                {
                    Invited = new User()
                    {
                        Name = "Jake"
                    },
                    DeclinedAt = DateTime.Now.AddHours(-1).AddMinutes(-5),
                    Status = IInvite.InviteStatus.Declined,
                },
            });
        }
    }
}
