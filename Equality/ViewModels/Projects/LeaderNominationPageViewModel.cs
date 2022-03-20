using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

using Catel.Collections;

using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using Equality.Data;

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
                    new () { Nominated = new () { Name = "user1" }, VotersCount = 4, PercentageSupport = 50},
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
                    item.PercentageSupport = (double)item.VotersCount / response.Object.Length * 100;

                    foreach (var voter in item.Voters) {
                        if (voter.IsCurrentUser) {
                            item.IsCurrentUserVotes = true;
                        }
                    }

                    NominatedMembers.Add(item);
                }

                Debug.WriteLine(NominatedMembers);

            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadLeaderNominationsAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
