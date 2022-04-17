using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.ExceptionHandling;

using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;
using Catel.MVVM;
using Equality.Helpers;
using System.Linq;

namespace Equality.ViewModels
{
    public class LeaderNominationPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public LeaderNominationPageViewModel()
        {
            HandleDesignMode(() =>
            {
                NominatedMembers.AddRange(new LeaderNomination[]
                {
                    new () { Nominated = new () { Name = "user1" }, VotersCount = 4, PercentageSupport = 50, IsLeader=true},
                    new () { Nominated = new () { Name = "user2" }, VotersCount = 3, PercentageSupport = 33, IsCurrentUserVotes=true},
                    new () { Nominated = new () { Name = "user3" }, VotersCount = 2, PercentageSupport = 10},
                    new () { Nominated = new () { Name = "user4" }, VotersCount = 1, PercentageSupport = 7},
                    new () { Nominated = new () { Name = "user5" }, VotersCount = 0, PercentageSupport = 0},
                    new () { Nominated = new () { Name = "user6" }, VotersCount = 0, PercentageSupport = 0},
                    new () { Nominated = new () { Name = "user7" }, VotersCount = 0, PercentageSupport = 0},
                });
            });
        }

        #endregion

        IProjectService ProjectService;

        public LeaderNominationPageViewModel(IProjectService projectService)
        {
            ProjectService = projectService;

            NominateUser = new TaskCommand<TeamMember>(OnNominateUserExecuteAsync);
        }

        #region Properties

        public ObservableCollection<LeaderNomination> NominatedMembers { get; set; } = new();

        #endregion

        #region Commands

        public TaskCommand<TeamMember> NominateUser { get; private set; }

        private async Task OnNominateUserExecuteAsync(TeamMember teamMember)
        {
            if (NominatedMembers.First(nomination => nomination.Nominated == teamMember).IsCurrentUserVotes) {
                return;
            }

            try {
                var response = await ProjectService.NominateUserAsync(StateManager.SelectedProject, teamMember, new()
                {
                    Includes = new[]
                    {
                        "nominated", "voters", "voters_count",
                    }
                });

                ProcessNominations(response.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        #endregion

        #region Methods

        protected async Task LoadLeaderNominationsAsync()
        {
            try {
                var response = await ProjectService.GetNominatedUsersAsync(StateManager.SelectedProject, new()
                {
                    Includes = new[]
                    {
                        "nominated", "voters", "voters_count",
                    }
                });

                ProcessNominations(response.Object);
            } catch (HttpRequestException e) {
                ExceptionHandler.Handle(e);
            }
        }

        protected void ProcessNominations(LeaderNomination[] nominations)
        {
            foreach (var item in nominations) {
                item.PercentageSupport = (double)item.VotersCount / nominations.Length * 100;
                item.IsCurrentUserVotes = item.Voters.Any(voter => voter.IsCurrentUser);
            }

            NominatedMembers.ReplaceRange(nominations);
            MvvmHelper.GetFirstInstanceOfViewModel<ProjectPageViewModel>().Leader = nominations.First(nomination => nomination.IsLeader).Nominated;
        }

        protected async Task SubscribePusherAsync()
        {
            await ProjectService.SubscribeNominateUserAsync(StateManager.SelectedProject, (LeaderNomination[] nominations) =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                    ProcessNominations(nominations);
                });
            });
        }

        protected async void UnsubscribePusher()
        {
            ProjectService.UnsubscribeNominateUser(StateManager.SelectedProject);
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadLeaderNominationsAsync();
            await Data.ExceptionHandler.Service.ProcessWithRetryAsync(SubscribePusherAsync);
        }

        protected override async Task CloseAsync()
        {
            UnsubscribePusher();

            await base.CloseAsync();
        }
    }
}
