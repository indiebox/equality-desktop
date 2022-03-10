using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Catel.Collections;
using Catel.MVVM;
using Catel.Services;

using Equality.Helpers;
using Equality.MVVM;
using Equality.Models;
using Equality.Services;
using System.Net.Http;
using System.Diagnostics;
using Equality.Data;

namespace Equality.ViewModels
{
    public class ProjectsPageViewModel : ViewModel
    {
        public bool IsFiltered = false;

        protected IUIVisualizerService UIVisualizerService;

        protected ITeamService TeamService;

        protected IProjectService ProjectService;

        public ProjectsPageViewModel(IUIVisualizerService uIVisualizerService, ITeamService teamService, IProjectService projectService)
        {
            UIVisualizerService = uIVisualizerService;
            TeamService = teamService;
            ProjectService = projectService;

            OpenProjectPage = new Command<Project>(OnOpenOpenProjectPageExecute);
            OpenCreateTeamWindow = new TaskCommand(OnOpenCreateTeamWindowExecute, () => CreateTeamVm is null);
            OpenTeamPage = new Command<Team>(OnOpenTeamPageExecute);
            FilterProjects = new Command<Team>(OnFilterProjectsExecute);
            ResetFilter = new Command(OnResetFilterExecute);

        }

        #region Properties

        public ObservableCollection<Team> Teams { get; set; } = new();

        public ObservableCollection<Team> FilteredTeams { get; set; } = new();

        public CreateTeamControlViewModel CreateTeamVm { get; set; }

        #endregion

        #region Commands

        public Command<Project> OpenProjectPage { get; private set; }

        private void OnOpenOpenProjectPageExecute(Project project)
        {
            var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
            StateManager.SelectedProject = project;
            vm.SelectedProject = project;
            vm.ActiveTab = ApplicationWindowViewModel.Tab.Project;
        }

        public TaskCommand OpenCreateTeamWindow { get; private set; }

        private async Task OnOpenCreateTeamWindowExecute()
        {
            CreateTeamVm = MvvmHelper.CreateViewModel<CreateTeamControlViewModel>();
            CreateTeamVm.ClosedAsync += CreateTeamVmClosedAsync;
        }

        public Command<Team> OpenTeamPage { get; private set; }

        private void OnOpenTeamPageExecute(Team team)
        {
            var vm = MvvmHelper.GetFirstInstanceOfViewModel<ApplicationWindowViewModel>();
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

        private Task CreateTeamVmClosedAsync(object sender, ViewModelClosedEventArgs e)
        {
            if (e.Result ?? false) {
                Teams.Add(CreateTeamVm.Team);

                if (!IsFiltered) {
                    FilteredTeams.Add(CreateTeamVm.Team);
                }
            }

            CreateTeamVm.ClosedAsync -= CreateTeamVmClosedAsync;
            CreateTeamVm = null;

            return Task.CompletedTask;
        }

        protected async void LoadTeamsAsync()
        {
            try {
                var response = await TeamService.GetTeamsAsync();
                Teams.AddRange(response.Object);
                FilteredTeams.AddRange(Teams);

                foreach (var team in response.Object) {
                    var responseProjects = await ProjectService.GetProjectsAsync(team);

                    team.Projects.AddRange(responseProjects.Object);
                }
            } catch (HttpRequestException e) {
                Debug.WriteLine(e.ToString());
            }
        }

        #endregion

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            LoadTeamsAsync();
        }

        protected override async Task CloseAsync()
        {
            if (CreateTeamVm != null) {
                CreateTeamVm.ClosedAsync -= CreateTeamVmClosedAsync;
            }

            await base.CloseAsync();
        }
    }
}
