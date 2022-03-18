using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.Services;

using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;

namespace Equality.ViewModels
{
    public class LeaderElectionPageViewModel : ViewModel
    {
        #region DesignModeConstructor

        public LeaderElectionPageViewModel()
        {
            HandleDesignMode(() =>
            {
                NominatedMembers.AddRange(new LeaderNomination[]
                {
                    new LeaderNomination() { Nominated = new User() { Name = "user1" }, Count = 4, PercentageSupport = 50, IsCurrentUserVotes=true},
                    new LeaderNomination() { Nominated = new User() { Name = "user2" }, Count = 3, PercentageSupport = 33},
                    new LeaderNomination() { Nominated = new User() { Name = "user3" }, Count = 2, PercentageSupport = 10},
                    new LeaderNomination() { Nominated = new User() { Name = "user4" }, Count = 1, PercentageSupport = 7},
                    new LeaderNomination() { Nominated = new User() { Name = "user5" }, Count = 0, PercentageSupport = 0},
                    new LeaderNomination() { Nominated = new User() { Name = "user6" }, Count = 0, PercentageSupport = 0},
                    new LeaderNomination() { Nominated = new User() { Name = "user7" }, Count = 0, PercentageSupport = 0},
                });
            });
        }

        #endregion

        IProjectService ProjectService;

        public LeaderElectionPageViewModel(INavigationService navigationService, IProjectService projectService)
        {
            ProjectService = projectService;
        }

        #region Properties

        public ObservableCollection<LeaderNomination> NominatedMembers { get; set; } = new();

        #endregion

        #region Commands



        #endregion

        #region Methods

        protected async Task LoadLeaderNominationsAsync()
        {
            try {
                var response = await ProjectService.GetNominatedUsersAsync(StateManager.SelectedProject);

                var result = response.Object;

                foreach (var item in result) {
                    //item.PercentageSupport = item.Count / response.Object.Length * 100;
                    item.PercentageSupport = 50;
                    foreach (var voter in item.Voters) {
                        if (voter.IsCurrentUser) {
                            item.IsCurrentUserVotes = true;
                        }
                    }
                    NominatedMembers.Add(item);
                }
                Debug.WriteLine(NominatedMembers[0]);

            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await LoadLeaderNominationsAsync();

            await base.InitializeAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
