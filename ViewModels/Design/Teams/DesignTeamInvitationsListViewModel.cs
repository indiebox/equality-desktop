using System;

using Equality.Models;

namespace Equality.ViewModels.Design
{
    public class DesignTeamInvitationsListViewModel : TeamInvitationsListViewModel
    {
        public DesignTeamInvitationsListViewModel() : base(null)
        {
            FilteredInvites = new()
            {
                new Invite()
                {
                    Inviter = new()
                    {
                        Name = "IgorGaming"
                    },
                    Invited = new()
                    {
                        Name = "RedMarshall"
                    },
                    CreatedAt = DateTime.Now,
                    Status = Invite.InviteStatus.Pending,
                },
                new Invite()
                {
                    Invited = new()
                    {
                        Name = "Borya"
                    },
                    AcceptedAt = DateTime.Now.AddMinutes(-3),
                    Status = Invite.InviteStatus.Accepted,
                },
                new Invite()
                {
                    Invited = new()
                    {
                        Name = "Jake"
                    },
                    DeclinedAt = DateTime.Now.AddHours(-1).AddMinutes(-5),
                    Status = Invite.InviteStatus.Declined,
                },
            };
        }
    }
}
