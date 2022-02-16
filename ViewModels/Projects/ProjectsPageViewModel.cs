using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Models;
using Catel.Collections;
using Equality.Services;
using System.Collections.Generic;

namespace Equality.ViewModels
{
    public class ProjectsPageViewModel : ViewModel
    {
        protected IUIVisualizerService UIVisualizerService;

        protected ITeamService TeamService;

        public ProjectsPageViewModel(IUIVisualizerService uIVisualizerService, ITeamService teamService)
        {
            UIVisualizerService = uIVisualizerService;
            TeamService = teamService;

            OpenCreateTeamWindow = new TaskCommand(OnOpenCreateTeamWindowExecute);
        }

        #region Properties

        public ObservableCollection<Team> Teams { get; set; } = new();

        #endregion

        #region Commands

        public TaskCommand OpenCreateTeamWindow { get; private set; }

        private async Task OnOpenCreateTeamWindowExecute()
        {
            Dictionary<string, Team> teamStorage = new();

            await UIVisualizerService.ShowAsync<CreateTeamDataWindowViewModel>(teamStorage);

            if (teamStorage.ContainsKey("Team")) {
                Teams.Add(teamStorage["Team"]);
            }
        }

        #endregion

        #region Methods

        protected async void LoadTeamsAsync()
        {
            var teams = await TeamService.GetTeamsAsync();

            Teams.AddRange(teams);
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadTeamsAsync();
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }
    }
}
