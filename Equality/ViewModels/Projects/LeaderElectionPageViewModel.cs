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
                    new LeaderNomination() { User = new User(){ Name = "user1" }, Count = 5, PercentageSupport = 50, Electorate = { StateManager.CurrentUser } },
                    new LeaderNomination() { User = new User(){ Name = "user2" }, Count = 3, PercentageSupport = 30, Electorate = { new User() }  },
                    new LeaderNomination() { User = new User(){ Name = "user3" }, Count = 2, PercentageSupport = 20, Electorate = { new User() }  },
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

                NominatedMembers.AddRange(response.Object);

            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            await LoadLeaderNominationsAsync();

            foreach (var member in NominatedMembers) {
                member.PercentageSupport = member.Count / NominatedMembers.Count * 100;
            }


            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
