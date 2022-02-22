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
    public class TeamMembersPageViewModel : ViewModel
    {
        protected Team Team;

        protected ITeamService TeamService;

        public TeamMembersPageViewModel(ITeamService teamService)
        {
            TeamService = teamService;

            ShowDialog = new TaskCommand(OnShowDialogExecute);
        }

        #region Properties

        public string FilterText { get; set; }

        public ObservableCollection<TeamMember> Members { get; set; } = new();

        public ObservableCollection<TeamMember> FilteredMembers { get; set; } = new();

        #endregion

        #region Commands

        public TaskCommand ShowDialog { get; private set; }

        private async Task OnShowDialogExecute()
        {
            var view = MvvmHelper.CreateViewWithViewModel<LeaveTeamDialogViewModel>(Members.Count == 1);
            bool result = (bool)await DialogHost.Show(view);

            if (result) {
                await LeaveTeam();
            }
        }

        #endregion

        #region Methods

        private void OnFilterTextChanged()
        {
            FilterMembers();
        }

        protected async Task LoadMembersAsync()
        {
            try {
                var response = await TeamService.GetMembersAsync(Team);

                Members.AddRange(response.Object);

                FilterMembers();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        protected async Task LeaveTeam()
        {
            try {
                await TeamService.LeaveTeamAsync(Team);

                var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
                vm.ActiveTab = ApplicationWindowViewModel.Tab.Main;
                vm.SelectedTeam = null;
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        protected void FilterMembers()
        {
            if (string.IsNullOrEmpty(FilterText)) {
                FilteredMembers.ReplaceRange(Members);

                return;
            }

            FilteredMembers.ReplaceRange(Members.Where(user => user.Name.ToLower().Contains(FilterText.ToLower())));
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            Team = MvvmHelper.GetFirstInstanceOfViewModel<TeamPageViewModel>().Team;

            await LoadMembersAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
