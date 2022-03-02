using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;

using Equality.Core.Helpers;
using Equality.Core.ViewModel;
using Equality.Models;
using Equality.Services;

using MaterialDesignThemes.Wpf;

namespace Equality.ViewModels
{
    public class TeamInvitationsListViewModel : ViewModel
    {
        protected Team Team;

        protected IInviteService InviteService;

        public TeamInvitationsListViewModel(IInviteService inviteService)
        {
            InviteService = inviteService;

            OpenInviteUserDialog = new TaskCommand(OnOpenInviteUserDialogExecuteAsync);

            NavigationCompleted += OnNavigated;
        }

        #region Properties

        public ObservableCollection<Invite> Invites { get; set; } = new();

        public ObservableCollection<Invite> FilteredInvites { get; set; } = new();

        public IInviteService.InviteFilter SelectedFilter { get; set; }

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

                if (SelectedFilter == IInviteService.InviteFilter.All
                    || SelectedFilter == IInviteService.InviteFilter.Pending) {
                    FilteredInvites.Add(invite);
                }
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
                case IInviteService.InviteFilter.All:
                default:
                    FilteredInvites.ReplaceRange(Invites);
                    break;
                case IInviteService.InviteFilter.Pending:
                    FilteredInvites.ReplaceRange(Invites.Where(invite => invite.Status == Invite.InviteStatus.Pending));
                    break;
                case IInviteService.InviteFilter.Accepted:
                    FilteredInvites.ReplaceRange(Invites.Where(invite => invite.Status == Invite.InviteStatus.Accepted));
                    break;
                case IInviteService.InviteFilter.Declined:
                    FilteredInvites.ReplaceRange(Invites.Where(invite => invite.Status == Invite.InviteStatus.Declined));
                    break;
            }
        }

        protected async Task LoadInvitesAsync()
        {
            try {
                var response = await InviteService.GetTeamInvitesAsync(Team);

                Invites.AddRange(response.Object);
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;

            await LoadInvitesAsync();

            SelectedFilter = IInviteService.InviteFilter.Pending;
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
