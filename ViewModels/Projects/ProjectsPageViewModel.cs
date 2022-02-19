﻿using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Catel.MVVM;
using Catel.Services;

using Equality.Core.ViewModel;
using Equality.Models;
using Catel.Collections;
using Equality.Services;
using System.Collections.Generic;
using System.Linq;
using Catel.IoC;

namespace Equality.ViewModels
{
    public class ProjectsPageViewModel : ViewModel
    {
        public bool IsFiltered = false;

        protected IUIVisualizerService UIVisualizerService;

        protected ITeamService TeamService;

        public ProjectsPageViewModel(IUIVisualizerService uIVisualizerService, ITeamService teamService)
        {
            UIVisualizerService = uIVisualizerService;
            TeamService = teamService;

            OpenCreateTeamWindow = new TaskCommand(OnOpenCreateTeamWindowExecute);
            OpenTeamPage = new Command<Team>(OnOpenTeamPageExecute);
            FilterProjects = new Command<Team>(OnFilterProjectsExecute);
            ResetFilter = new Command(OnResetFilterExecute);
        }

        #region Properties

        public ObservableCollection<Team> Teams { get; set; } = new();

        public ObservableCollection<Team> FilteredTeams { get; set; } = new();

        #endregion

        #region Commands

        public TaskCommand OpenCreateTeamWindow { get; private set; }

        private async Task OnOpenCreateTeamWindowExecute()
        {
            Dictionary<string, Team> teamStorage = new();

            await UIVisualizerService.ShowAsync<CreateTeamDataWindowViewModel>(teamStorage);

            if (teamStorage.ContainsKey("Team")) {
                Teams.Add(teamStorage["Team"]);

                if (!IsFiltered) {
                    FilteredTeams.Add(teamStorage["Team"]);
                }
            }
        }

        public Command<Team> OpenTeamPage { get; private set; }

        private void OnOpenTeamPageExecute(Team team)
        {
            var vmmanager = this.GetDependencyResolver().Resolve<IViewModelManager>();
            var vm = vmmanager.GetFirstOrDefaultInstance<ApplicationWindowViewModel>();
            vm.SelectedTeam = team;
            vm.ActiveTab = ApplicationWindowViewModel.Tab.Team;
        }

        public Command<Team> FilterProjects { get; private set; }

        private void OnFilterProjectsExecute(Team filterByTeam)
        {
            IsFiltered = true;

            FilteredTeams.ReplaceRange(Teams.Where(team => team == filterByTeam));
        }

        public Command ResetFilter { get; private set; }

        private void OnResetFilterExecute()
        {
            IsFiltered = false;

            FilteredTeams.ReplaceRange(Teams);
        }

        #endregion

        #region Methods

        protected async void LoadTeamsAsync()
        {
            var response = await TeamService.GetTeamsAsync();

            Teams.AddRange(response.Object);
            FilteredTeams.AddRange(Teams);
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