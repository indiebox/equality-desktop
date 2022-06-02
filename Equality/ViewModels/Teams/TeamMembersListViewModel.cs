using System;
using System.Collections.ObjectModel;
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
using Equality.Http;
using Catel.Fody;

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
                Members.AddRange(new TeamMember[]
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

            LoadMoreMembers = new(OnLoadMoreMembersExecuteAsync, () => MembersPaginator?.HasNextPage ?? false);
            LeaveTeam = new TaskCommand(OnLeaveTeamExecute);
            InviteUser = new TaskCommand(OnInviteUserExecuteAsync);
        }

        #region Properties

        public string FilterText { get; set; }

        private async void OnFilterTextChanged()
        {
            var text = FilterText?.ToLower()?.Trim();
            if (text == CurrentFilter) {
                return;
            }

            CurrentFilter = text;
            await LoadMembersAsync();
        }

        [NoWeaving]
        public string CurrentFilter { get; set; }

        public ObservableCollection<TeamMember> Members { get; set; } = new();

        public PaginatableApiResponse<TeamMember> MembersPaginator { get; set; }

        #endregion

        #region Commands

        public TaskCommand LoadMoreMembers { get; private set; }

        private async Task OnLoadMoreMembersExecuteAsync()
        {
            try {
                MembersPaginator = await MembersPaginator.NextPageAsync();
                Members.AddRange(MembersPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        public TaskCommand LeaveTeam { get; private set; }

        private async Task OnLeaveTeamExecute()
        {
            var view = MvvmHelper.CreateViewWithViewModel<LeaveTeamDialogViewModel>(Members.Count == 1);
            bool result = (bool)await DialogHost.Show(view);

            if (!result) {
                return;
            }

            try {
                await TeamService.LeaveTeamAsync(StateManager.SelectedTeam);

                StateManager.SelectedTeam = null;

                var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
                vm.ActiveTab = ApplicationWindowViewModel.Tab.Main;
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
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

        protected async Task LoadMembersAsync()
        {
            var query = new QueryParameters();

            if (!string.IsNullOrWhiteSpace(CurrentFilter)) {
                query.Filters = new[] { new Filter("name", CurrentFilter) };
            }

            try {
                MembersPaginator = await TeamService.GetMembersAsync(StateManager.SelectedTeam, query);

                Members.ReplaceRange(MembersPaginator.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
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
