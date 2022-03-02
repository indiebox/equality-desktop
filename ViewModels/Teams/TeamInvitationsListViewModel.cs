using System.Threading.Tasks;

using Catel.MVVM;

using Equality.Core.Helpers;
using Equality.Core.ViewModel;
using Equality.Models;

using MaterialDesignThemes.Wpf;

namespace Equality.ViewModels
{
    public class TeamInvitationsListViewModel : ViewModel
    {
        protected Team Team;

        public TeamInvitationsListViewModel()
        {
            OpenInviteUserDialog = new TaskCommand(OnOpenInviteUserDialogExecuteAsync);

            NavigationCompleted += OnNavigated;
        }

        #region Properties


        #endregion

        #region Commands

        public TaskCommand OpenInviteUserDialog { get; private set; }

        private async Task OnOpenInviteUserDialogExecuteAsync()
        {
            var invite = new Invite();
            var view = MvvmHelper.CreateViewWithViewModel<InviteUserDialogViewModel>(invite);
            bool result = (bool)await DialogHost.Show(view);

            if (result) {
                // invite successfully sended
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

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;
        }

        protected override async Task CloseAsync()
        {
            await base.CloseAsync();
        }
    }
}
