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

namespace Equality.ViewModels
{
    public class TeamMembersListViewModel : ViewModel
    {
        protected ITeamService TeamService;

        #region DesignModeConstructor

        public TeamMembersListViewModel()
        {
            HandleDesignMode(() =>
            {
                FilteredMembers.AddRange(new TeamMember[]
                {
                    new TeamMember() { Name = "user1", JoinedAt = DateTime.Now.AddHours(-2) },
                    new TeamMember() { Name = "user2", JoinedAt = DateTime.Now.AddDays(-1).AddHours(-1) },
                    new TeamMember() { Name = "user3", JoinedAt = DateTime.Now.AddDays(-1).AddHours(-4) },
                    new TeamMember() { Name = "Пользователь 4", JoinedAt = DateTime.Now.AddDays(-2).AddHours(-5) },
                    new TeamMember() { Name = "Пользователь 5", JoinedAt = DateTime.Now.AddDays(-3).AddHours(-5) },
                    new TeamMember() { Id = 1, Name = "Текущий пользователь", JoinedAt = DateTime.Now.AddDays(-3).AddHours(-5) },
                });
            });
        }

        #endregion

        public TeamMembersListViewModel(ITeamService teamService)
        {
            TeamService = teamService;

            ShowDialog = new TaskCommand(OnShowDialogExecute);
            InviteUser = new TaskCommand(OnInviteUserExecuteAsync);
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

        public TaskCommand InviteUser { get; private set; }

        private async Task OnInviteUserExecuteAsync()
        {
            var membersPage = MvvmHelper.GetFirstInstanceOfViewModel<TeamMembersPageViewModel>();
            membersPage.NavigationParameters = new() { { "send-invite", true } };
            membersPage.ActiveTab = TeamMembersPageViewModel.Tab.Invitations;
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
                var response = await TeamService.GetMembersAsync(StateManager.SelectedTeam);

                Members.AddRange(response.Object);

                FilterMembers();
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        protected async Task LeaveTeam()
        {
            try {
                await TeamService.LeaveTeamAsync(StateManager.SelectedTeam);

                StateManager.SelectedTeam = null;

                var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
                vm.ActiveTab = ApplicationWindowViewModel.Tab.Main;
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

            await LoadMembersAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
