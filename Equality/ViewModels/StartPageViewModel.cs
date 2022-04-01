using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;

using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;

namespace Equality.ViewModels
{
    public class StartPageViewModel : ViewModel
    {
        protected IInviteService InviteService;

        #region DesignModeConstructor

        public StartPageViewModel()
        {
            HandleDesignMode(() =>
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
            });
        }

        #endregion

        public StartPageViewModel(IInviteService inviteService)
        {
            InviteService = inviteService;

            AcceptInvite = new TaskCommand<Invite>(OnAcceptInviteExecuteAsync);
            DeclineInvite = new TaskCommand<Invite>(OnDeclineInviteExecuteAsync);

            Name = StateManager.CurrentUser.Name;
        }

        #region Properties

        public string Name { get; set; }

        public ObservableCollection<Invite> Invites { get; set; } = new();

        #endregion

        #region Commands

        public TaskCommand<Invite> AcceptInvite { get; private set; }

        private async Task OnAcceptInviteExecuteAsync(Invite invite)
        {
            try {
                await InviteService.AcceptInviteAsync(invite);

                Invites.Remove(invite);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public TaskCommand<Invite> DeclineInvite { get; private set; }

        private async Task OnDeclineInviteExecuteAsync(Invite invite)
        {
            try {
                await InviteService.DeclineInviteAsync(invite);

                Invites.Remove(invite);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Methods

        protected async Task LoadInvitesAsync()
        {
            try {
                var response = await InviteService.GetUserInvitesAsync();

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
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
