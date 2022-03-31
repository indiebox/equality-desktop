using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;

using Equality.Helpers;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;

using MaterialDesignThemes.Wpf;
using System;
using Equality.Http;

namespace Equality.ViewModels
{
    public class TeamInvitationsListViewModel : ViewModel
    {
        protected IInviteService InviteService;

        #region DesignModeConstructor

        public TeamInvitationsListViewModel()
        {
            HandleDesignMode(() =>
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
            });
        }

        #endregion

        public TeamInvitationsListViewModel(IInviteService inviteService)
        {
            InviteService = inviteService;

            OpenInviteUserDialog = new TaskCommand(OnOpenInviteUserDialogExecuteAsync);
            RevokeInvite = new TaskCommand<Invite>(OnRevokeInviteExecuteAsync);

            NavigationCompleted += OnNavigated;
        }

        public enum InviteFilter
        {
            All,
            Pending,
            Accepted,
            Declined,
        };

        #region Properties

        public ObservableCollection<Invite> Invites { get; set; } = new();

        public ObservableCollection<Invite> FilteredInvites { get; set; } = new();

        public InviteFilter SelectedFilter { get; set; }

        #endregion

        #region Commands

        public TaskCommand OpenInviteUserDialog { get; private set; }

        private async Task OnOpenInviteUserDialogExecuteAsync()
        {
            var invite = new Invite();
            var view = MvvmHelper.CreateViewWithViewModel<InviteUserDialogViewModel>(invite);
            bool result = (bool)await DialogHost.Show(view);

            if (result) {
                Invites.Add(invite);

                if (SelectedFilter == InviteFilter.All
                    || SelectedFilter == InviteFilter.Pending) {
                    FilteredInvites.Add(invite);
                }
            }
        }

        public TaskCommand<Invite> RevokeInvite { get; private set; }

        private async Task OnRevokeInviteExecuteAsync(Invite invite)
        {
            try {
                await InviteService.RevokeInviteAsync(invite);

                Invites.Remove(invite);
                FilteredInvites.Remove(invite);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Methods

        private void OnNavigated(object sender, System.EventArgs e)
        {
            if (NavigationContext.Values.ContainsKey("send-invite")) {
                OpenInviteUserDialog.Execute();
            }
        }

        private void OnSelectedFilterChanged()
        {
            switch (SelectedFilter) {
                case InviteFilter.All:
                default:
                    FilteredInvites.ReplaceRange(Invites);
                    break;
                case InviteFilter.Pending:
                    FilteredInvites.ReplaceRange(Invites.Where(invite => invite.Status == IInvite.InviteStatus.Pending));
                    break;
                case InviteFilter.Accepted:
                    FilteredInvites.ReplaceRange(Invites.Where(invite => invite.Status == IInvite.InviteStatus.Accepted));
                    break;
                case InviteFilter.Declined:
                    FilteredInvites.ReplaceRange(Invites.Where(invite => invite.Status == IInvite.InviteStatus.Declined));
                    break;
            }
        }

        protected async Task LoadInvitesAsync()
        {
            try {
                var response = await InviteService.GetTeamInvitesAsync(StateManager.SelectedTeam, new()
                {
                    Fields = new[]
                    {
                        new Field("invites", "id", "status", "accepted_at", "declined_at", "created_at")
                    }
                });

                Invites.AddRange(response.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadInvitesAsync();

            SelectedFilter = InviteFilter.Pending;
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
