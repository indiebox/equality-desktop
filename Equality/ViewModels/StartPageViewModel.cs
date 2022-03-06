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
                Debug.WriteLine(e.ToString());
            }
        }

        public TaskCommand<Invite> DeclineInvite { get; private set; }

        private async Task OnDeclineInviteExecuteAsync(Invite invite)
        {
            try {
                await InviteService.DeclineInviteAsync(invite);

                Invites.Remove(invite);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
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
                Debug.WriteLine(e.ToString());
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
