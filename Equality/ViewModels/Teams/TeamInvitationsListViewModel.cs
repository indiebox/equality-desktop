using System.Collections.ObjectModel;
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
    /*
     * NavigationContext: 
     * send-invite - open invite user dialog
     */
    public class TeamInvitationsListViewModel : ViewModel
    {
        protected IInviteService InviteService;

        #region DesignModeConstructor

        public TeamInvitationsListViewModel()
        {
            HandleDesignMode(() =>
            {
                Invites.AddRange(new Invite[]
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

            LoadMoreInvites = new(OnLoadMoreInvitesExecuteAsync, () => InvitesPaginator?.HasNextPage ?? false);
            InviteUser = new TaskCommand(OnOpenInviteUserDialogExecuteAsync);
            RevokeInvite = new TaskCommand<Invite>(OnRevokeInviteExecuteAsync);
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

        public PaginatableApiResponse<Invite> InvitesPaginator { get; set; }

        public InviteFilter SelectedFilter { get; set; }

        private async void OnSelectedFilterChanged()
        {
            await LoadInvitesAsync(SelectedFilter);
        }

        #endregion

        #region Commands

        public TaskCommand LoadMoreInvites { get; private set; }

        private async Task OnLoadMoreInvitesExecuteAsync()
        {
            try {
                InvitesPaginator = await InvitesPaginator.NextPageAsync();

                if (IsClosed) {
                    return;
                }

                Invites.AddRange(InvitesPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public TaskCommand InviteUser { get; private set; }

        private async Task OnOpenInviteUserDialogExecuteAsync()
        {
            var invite = new Invite();
            var view = MvvmHelper.CreateViewWithViewModel<InviteUserDialogViewModel>(invite);
            bool result = (bool)await DialogHost.Show(view);

            if (result) {
                if (SelectedFilter == InviteFilter.All
                    || SelectedFilter == InviteFilter.Pending) {
                    Invites.Insert(0, invite);
                }
            }
        }

        public TaskCommand<Invite> RevokeInvite { get; private set; }

        private async Task OnRevokeInviteExecuteAsync(Invite invite)
        {
            try {
                await InviteService.RevokeInviteAsync(invite);

                Invites.Remove(invite);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Methods

        protected override void OnNavigationCompleted()
        {
            if (NavigationContext.Values.ContainsKey("send-invite")) {
                InviteUser.Execute();
            }
        }

        protected async Task LoadInvitesAsync(InviteFilter filter = InviteFilter.Pending)
        {
            try {
                InvitesPaginator = await InviteService.GetTeamInvitesAsync(StateManager.SelectedTeam, new()
                {
                    Fields = new[]
                    {
                        new Field("invites", "id", "status", "accepted_at", "declined_at", "created_at")
                    },
                    Filters = new[] { new Filter("status", filter.ToString().ToLower()) },
                });

                if (IsClosed) {
                    return;
                }

                Invites.ReplaceRange(InvitesPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            SelectedFilter = InviteFilter.Pending;
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
